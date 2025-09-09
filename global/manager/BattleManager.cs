using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class BattleManager : Node{
	private Queue<Fighter> turnQueue = new();
	private Fighter currentFighter;
	private FighterTeam teamPlayer;
	private FighterTeam teamEnemy;
	
	private bool[] wasFaintedPlayer;
	private bool[] wasFaintedEnemy;
	
	private CustomSignals customSignals;
	
	private int pendingEffectCount = 0;
	private int completedEffectCount = 0;
	private TaskCompletionSource<bool> allEffectsDoneTcs;
	
	private int battleExpGain = 0;
	private int battleCoinsGain = 0;
	
	public override void _Ready(){
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.OnTargetsConfirmed += ExecuteTurnAction;
		customSignals.OnEffectIsDone += OnEffectIsDoneFromFighter;
	}

	public void StartBattle(FighterTeam playerTeam, FighterTeam enemyTeam){
		if (customSignals == null)
			customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		teamPlayer = playerTeam;
		teamEnemy = enemyTeam;
		wasFaintedPlayer = new bool[playerTeam.GetFighters().Count];
		wasFaintedEnemy = new bool[enemyTeam.GetFighters().Count];
		battleExpGain = 0;
		battleCoinsGain = 0;
		GenerateTurnOrder();
		StartNextTurn();
	}

	private void GenerateTurnOrder()
	{
		List<Fighter> allFighters = new List<Fighter>();
		allFighters.AddRange(teamPlayer.GetFighters());
		allFighters.AddRange(teamEnemy.GetFighters());

		// Ejemplo: ordenados por una stat de velocidad
		allFighters.Sort((a, b) => 
			b.GetEntityData().GetEffectiveSpeed().CompareTo(a.GetEntityData().GetEffectiveSpeed())
		);

		foreach (var fighter in allFighters)
			turnQueue.Enqueue(fighter);
	}
	
	private void ReorderTurnQueue(){
		List<Fighter> remainingFighters = new List<Fighter>(turnQueue);
		// Ordenar por velocidad efectiva descendente
		remainingFighters.Sort((a, b) =>
			b.GetEntityData().GetEffectiveSpeed().CompareTo(a.GetEntityData().GetEffectiveSpeed())
		);
		turnQueue.Clear();
		foreach (var fighter in remainingFighters)
			turnQueue.Enqueue(fighter);
	}
	

	private void StartNextTurn(){
		if (turnQueue.Count == 0)
			GenerateTurnOrder(); // nuevo ciclo de turnos
		currentFighter = turnQueue.Dequeue();
		if (turnQueue.Count != 0)
			ReorderTurnQueue();
		if (currentFighter.IsDead()){
			StartNextTurn();
			return; //No quitar o se rompe!
		}
		if (currentFighter.IsPlayerControlled()){
			customSignals.EmitSignal(nameof(CustomSignals.OnPrepareBattleMenuForFighter),currentFighter,teamPlayer,teamEnemy);
			customSignals.EmitSignal(nameof(CustomSignals.OnShowFighterInAction),currentFighter);
		}
		else{
			// Turno IA
			currentFighter.ExecuteAITurn(teamEnemy,teamPlayer);
		}
	}
	
	private void CheckBattleState(){
		if (teamPlayer.AllFightersDead()){
			GD.Print("Derrota...");
			// Emitir señal de derrota
		}
		else if (teamEnemy.AllFightersDead()){
			GD.Print("Victoria!");
			turnQueue.Clear();
			teamEnemy.FreeTeam();
			GD.Print("EnemyTeam freed");
			// Emitir señal de victoria
			customSignals.EmitSignal(nameof(CustomSignals.OnVictory), battleExpGain, battleCoinsGain);
		}
		else{
			StartNextTurn();
		}
	}
	
	public async void ExecuteTurnAction(Fighter actor, Fighter[] targets, Skill skill){
		await ToSignal(customSignals, "OnDialogIsOver");
		actor.GetEntityData().removeMP(skill.GiveCost());
		actor.UpdateBars();
		int effectsToWaitFor = 0;
		// Primera parte: daño u otros efectos base
		for(int i = 0; i < targets.Length; i++){
			bool applied = skill.Execute1(actor, targets[i], this);
			if (applied){
				effectsToWaitFor++;
			}
		}
		if (effectsToWaitFor > 0){
			await WaitForAllEffectsDone(effectsToWaitFor);
		}
		// Segunda parte: efectos secundarios
		actor.UpdateBars();
		if(skill.HasSecondaryEffect()){
			effectsToWaitFor = 0;
			for(int i = 0; i < targets.Length; i++){
				bool applied = skill.Execute2(actor, targets[i], this);
				if(applied){
					effectsToWaitFor++;
				}
			}
			if (effectsToWaitFor > 0){
				await WaitForAllEffectsDone(effectsToWaitFor);
			}
		}
		// Tercera parte: efectos de los efectos de estado
		if(actor.GetEffectController().ThereIsActiveStatus()){
			await currentFighter.GetEffectController().ProcessEffects(this);
		}
		actor.UpdateStatusIcons();
		// Cuarta parte: revisar si alguien ha muerto en este turno
		List<Fighter> teamEnemyList = teamEnemy.GetFighters();
		List<Fighter> teamPlayerList = teamPlayer.GetFighters();
		for(int i = 0; i < teamEnemyList.Count; i++){
			if(teamEnemyList[i].IsDead() && !wasFaintedEnemy[i]){
				wasFaintedEnemy[i] = true;
				teamEnemyList[i].Faints();
				customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"{teamEnemyList[i].GetEntityData().Name} ha caido.");
				await ToSignal(customSignals, "OnDialogIsOver");
				EnemyIsDefeated(teamEnemyList[i]);
			}
		}
		for(int i = 0; i < teamPlayerList.Count; i++){
			if(teamPlayerList[i].IsDead() && !wasFaintedPlayer[i]){
				wasFaintedPlayer[i] = true;
				teamPlayerList[i].Faints();
				customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"{teamPlayerList[i].GetEntityData().Name} ha caido.");
				await ToSignal(customSignals, "OnDialogIsOver");
			}
		}
		CheckBattleState();
	}
	public async Task ExplosionCastedFromFighterOnATeam(Fighter f, int potenciaExplosion){
		List<Fighter> teamtarget;
		if(f.GetEntityData().isControlled()) teamtarget = teamEnemy.GetFighters();
		else teamtarget = teamPlayer.GetFighters();
		int effectsToWaitFor = 0;
		for(int i = 0; i < teamtarget.Count; i++){
			int damage = Skill.CalculateDamage(potenciaExplosion, f, teamtarget[i]);
			teamtarget[i].TakeDamageAnonymous(damage);
			effectsToWaitFor++;
		}
		if (effectsToWaitFor > 0){
			await WaitForAllEffectsDone(effectsToWaitFor);
		}
	}
	
	private Task WaitForAllEffectsDone(int totalToWait){
		pendingEffectCount = totalToWait;
		completedEffectCount = 0;
		allEffectsDoneTcs = new TaskCompletionSource<bool>();
		return allEffectsDoneTcs.Task;
	}
	public void OnEffectIsDoneFromFighter(){
		completedEffectCount++;
		if (completedEffectCount >= pendingEffectCount){
			allEffectsDoneTcs?.TrySetResult(true);
		}
	}
	
	public void EnemyIsDefeated(Fighter f){
		battleExpGain += f.DefeatedGivesExp();
		battleCoinsGain += f.DefeatedGivesCoins();
	}
}
