using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public partial class Fighter : Node2D{
	//[Export]private Entity data_Info;
	private Entity data_Info;
	private AnimatedSprite2D sprites;
	private AudioStreamPlayer2D playSounds;
	private SpritesManager sManager;
	private StatusEffectController statusController;
	private AICombatManager aiManager;
	private HealthBar healthBar;
	private ManaBar manaBar;
	private CustomSignals customSignals;
	
	private PackedScene PopUpTextScene;
	private List<Skill> skills;
	
	public override void _Ready(){
		sprites = GetNode<AnimatedSprite2D>("Sprites");
		sManager = GetNode<SpritesManager>("/root/SpritesManager");
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		PopUpTextScene = ResourceLoader.Load<PackedScene>("res://global/scenes/scenesUI/pop_up_text.tscn");
		statusController = GetNode<StatusEffectController>("StatusEffectController");
		aiManager = GetNode<AICombatManager>("AI");
		playSounds = GetNode<AudioStreamPlayer2D>("PlaySounds");
		aiManager.GiveFighterSkills(skills);
		healthBar = GetNode<HealthBar>("HealthBar");
		manaBar = GetNode<ManaBar>("ManaBar");
		if (data_Info != null){
			sprites.SpriteFrames = sManager.GenerateSpriteFrames(data_Info, data_Info.SpriteSheet);
			sprites.Play("idle"); // Empieza la animación "idle"
			UpdateBars();
		}
	}
	
	public void ExecuteAITurn(FighterTeam hisTeam, FighterTeam vsTeam){
		switch(data_Info.beh_type){
			case Behaviour.Aleatorio:
				aiManager.AIRandomDecision(hisTeam,vsTeam);
				break;
			case Behaviour.Agresivo:
				aiManager.AIAgresiveDecision(hisTeam,vsTeam);
				break;
			case Behaviour.Tactico:
				aiManager.AITacticDecision(hisTeam,vsTeam);
				break;
			case Behaviour.Apoyo:
				break;
		}
	}
	
	public void PlayAnimationSafe(string animationName) {
		if (sprites != null && sprites.SpriteFrames != null) {
			if (sprites.SpriteFrames.HasAnimation(animationName)) {
				sprites.Play(animationName);
			} else {
				GD.Print($"[WARNING] Animación '{animationName}' no encontrada para {data_Info?.Name ?? "Fighter"}.");
				// Si quieres reproducir "idle" por defecto si no existe la animación:
				if (sprites.SpriteFrames.HasAnimation("idle"))
					sprites.Play("idle");
			}
		}
	}
	public void StartBlink(){
		sManager.StartBlinking(this.sprites);
	}
	public void StopBlink(){
		sManager.StopBlinking(this.sprites);
	}
	public void Faints(){
		statusController.RemoveAllEffects();
		UpdateStatusIcons();
		if(IsPlayerControlled())	PlayAnimationSafe("fainted");
		else	sManager.PlayDeathTween(this);
	}
	private async Task ShowDamagePopup(int damage){
		if(IsDead()){
			PlayAnimationSafe("fainted");
		}else{
			PlayAnimationSafe("damaged");
		}
		var popup = (PopUpText)PopUpTextScene.Instantiate();
		GetTree().CurrentScene.AddChild(popup);

		// Posicionarlo encima del personaje
		Vector2 globalPosition = GetGlobalPosition();
		popup.GlobalPosition = globalPosition + new Vector2(0, -50); // un poco arriba del personaje
		popup.TheDamage(damage);
		await ToSignal(customSignals, "OnPopUpExpired");
		if(IsCritical()){
			TTS.PutThisInQueue($"{data_Info.Name} se estremece del dolor.");
			PlayAnimationSafe("idle_low_health");
		}else{
			if(IsDead()){
				PlayAnimationSafe("faited");
			}else{
				PlayAnimationSafe("idle");
			}
		}
	}
	private async Task ShowEffectPopup(string text){
		var popup = (PopUpText)PopUpTextScene.Instantiate();
		GetTree().CurrentScene.AddChild(popup);

		// Posicionarlo encima del personaje
		Vector2 globalPosition = GetGlobalPosition();
		popup.GlobalPosition = globalPosition + new Vector2(0, -50); // un poco arriba del personaje
		popup.TheEffect(text);
		await ToSignal(customSignals, "OnPopUpExpired");
	}
	public void UpdateStatusIcons(){
		List<StatusEffect> status = statusController.GetActiveStatus();
		List<Texture2D> icons = new List<Texture2D>();
		if(status.Count > 0){
			foreach (StatusEffect s in status){
				Texture2D icon = StatusHelper.GetIcono(s.Type);
				if (icon != null){
					icons.Add(icon);
				}
			}
		}
		GetNode<StatusDisplay>("StatusDisplay").SetIconos(icons);
	}
	public void UpdateBars(){
		healthBar.UpdateHealthBar();
		manaBar.UpdateManaBar();
	}
	public bool IsStunned(){
		return statusController.HasEffect(StatusEffectType.Aturdido);
	}
	public bool IsBlocked(){
		return statusController.HasEffect(StatusEffectType.Bloqueo);
	}
	public bool IsSealed(){
		return statusController.HasEffect(StatusEffectType.Sellado);
	}
	public bool IsMarked(){
		return statusController.HasEffect(StatusEffectType.Marca_del_cazador);
	}
	public bool IsPoisoned(){
		return statusController.HasEffect(StatusEffectType.Envenenado);
	}
	public bool IsBleeding(){
		return statusController.HasEffect(StatusEffectType.Sangrado);
	}
	public bool IsCritical(){
		return data_Info.criticalHealth();
	}
	public bool IsWeaving(){
		return statusController.HasEffect(StatusEffectType.Evasion);
	}
	public bool IsProtecting(){
		return statusController.HasEffect(StatusEffectType.Vanguardia);
	}
	
	public void Revive(){
		data_Info.restoreHP(1);
		UpdateBars();
		PlayAnimationSafe("idle_low");
	}
	public void FullMana(){
		data_Info.fullMP();
		UpdateBars();
		//Deberia esperar a una animacion
	}
	public void FullHealth(){
		data_Info.fullHP();
		UpdateBars();
		PlayAnimationSafe("idle");
		//Deberia esperar a una animacion
	}
	public async void HealAction(int n){
		data_Info.restoreHP(n);
		UpdateBars();
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/sound1.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowDamagePopup(n);
		customSignals.OnDialogIsOver += EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"{data_Info.Name} ha recuperado vida.");
		//Deberia esperar a una animacion
	}
	public async void Heal(int n){
		data_Info.restoreHP(n);
		UpdateBars();
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/sound1.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowDamagePopup(n);
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
		//Deberia esperar a una animacion
	}
	public async void RestoreMana(int n){
		data_Info.restoreMP(n);
		UpdateBars();
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/sound1.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowDamagePopup(n);
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
		//Deberia esperar a una animacion
	}
	public async void TakeDamage(int n, Fighter hitman){
		data_Info.removeHP(n);
		UpdateBars();
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/undertale-sound-effect-attack-hit.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowDamagePopup(n);
		aiManager.GotHitByFighter(hitman);
		TTS.PutThisInQueue($"{data_Info.Name} sufre {n} de daño.");
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
		//Deberia esperar a una animacion
	}
	public async void TakeDamageAnonymous(int n){
		data_Info.removeHP(n);
		UpdateBars();
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/undertale-sound-effect-attack-hit.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowDamagePopup(n);
		TTS.PutThisInQueue($"{data_Info.Name} sufre {n} de daño.");
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
		//Deberia esperar a una animacion
	}
	public async void TakeMultipleHits(int n, int ptg, int guaranteed, int limit, Fighter hitman){
		int count = 0;
		while(count < limit && !IsDead()){
			if(count < guaranteed){
				data_Info.removeHP(n);
				UpdateBars();
				AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/undertale-sound-effect-attack-hit.mp3");
				playSounds.Stream = sound;
				playSounds.Play();
				await ShowDamagePopup(n);
			}else{
				if(Skill.ProducesEffect(ptg+(count-guaranteed)*10)){
					data_Info.removeHP(n);
					UpdateBars();
					AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/undertale-sound-effect-attack-hit.mp3");
					playSounds.Stream = sound;
					playSounds.Play();
					await ShowDamagePopup(n);
					TTS.PutThisInQueue($"{data_Info.Name} sufre {n} de daño.");
				}
				else
					break;
			}
			count++;
		}
		aiManager.GotHitByFighter(hitman);
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
		//Deberia esperar a una animacion
	}
	public async void LosesMana(int n){
		data_Info.restoreMP(n);
		UpdateBars();
		await ShowDamagePopup(n);
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
		//Deberia esperar a una animacion
	}
	public async void ApplyStatus(StatusEffect effect){
		statusController.AddEffect(effect);
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/statusSound.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowEffectPopup(effect.StatusName);
		UpdateStatusIcons();
		customSignals.OnDialogIsOver += EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"{data_Info.Name} ha sido afectado por {effect.StatusName}.");
	}
	public async void ApplyMultipleStatus(StatusEffect[] effects){
		for(int i = 0; i < effects.Length; i++){
			statusController.AddEffect(effects[i]);
		}
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/statusSound.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowEffectPopup("Multiples estados");
		UpdateStatusIcons();
		UpdateBars();
		customSignals.OnDialogIsOver += EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnAffectedByMultipleEffects),data_Info.Name,effects);
	}
	public async void ResetStatus(){
		statusController.ClearAllEffects();
		await ShowEffectPopup("Reset");
		UpdateStatusIcons();
		UpdateBars();
		customSignals.OnDialogIsOver += EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"Todos los estados de {data_Info.Name} han desaparecido.");
		//Deberia esperar a una animacion
	}
	public async void RestoreGoodStatus(){
		statusController.ClearNegativeEffects();
		await ShowEffectPopup("Restauracion");
		UpdateStatusIcons();
		UpdateBars();
		customSignals.OnDialogIsOver += EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"Todos los estados negativos de {data_Info.Name} han desaparecido.");
		//Deberia esperar a una animacion
	}
	public async void DodgedAttack(){
		AudioStream sound = GD.Load<AudioStream>("res://assets/sonidos/Alex noises/weave.mp3");
		playSounds.Stream = sound;
		playSounds.Play();
		await ShowEffectPopup("Nope!");
		customSignals.OnDialogIsOver += EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), $"{data_Info.Name} ha esquivado el ataque.");
	}
	public void EffectIsDone(){
		customSignals.OnDialogIsOver -= EffectIsDone;
		customSignals.EmitSignal(nameof(CustomSignals.OnEffectIsDone));
	}
	
	public Entity GetEntityData(){
		return data_Info;
	}
	public void LevelUp(){
		data_Info.levelUp();
		for(int i = 0; i < skills.Count; i++){
			skills[i].LevelUp();
		}
	}
	public void AsignLevel(int l){
		data_Info.AssignLevel(l);
		for(int i = 0; i < skills.Count; i++){
			skills[i].AssingLevel(l);
		}
	}
	public void SetEntityData(Entity entity){
		data_Info = entity;
	}
	public void SetSkills(List<Skill> s){
		skills = s;
	}
	
	public Skill AtqBasico(){
		return skills[0];
	}
	public Skill Guardia(){
		if(skills.Count >= 2)
			return skills[1];
		return null;
	}
	public Skill Skill1(){
		if(skills.Count >= 3)
			return skills[2];
		return null;
	}
	public Skill Skill2(){
		if(skills.Count >= 4)
			return skills[3];
		return null;
	}
	public Skill Skill3(){
		if(skills.Count >= 5)
			return skills[4];
		return null;
	}
	public Skill Skill4(){
		if(skills.Count >= 6)
			return skills[5];
		return null;
	}
	
	public bool IsDead(){
		return data_Info.isDead();
	}
	public bool IsPlayerControlled(){
		return data_Info.isControlled();
	}
	public StatusEffectController GetEffectController(){
		return statusController;
	}
	public void FreeFighter(){
		aiManager.FreeAIManager();
		skills.Clear();
		QueueFree();
	}
	public int DefeatedGivesExp(){
		if(IsPlayerControlled()) return 0;
		return data_Info.giveExp();
	}
	public int DefeatedGivesCoins(){
		if(IsPlayerControlled()) return 0;
		return data_Info.giveCoins();
	}
	
	public string FighterShowBattleInfo(){
		string res = $"Luchador: {data_Info.Name}.";
		res = $"{res} Vida: {data_Info.giveHP()} de {data_Info.giveMAXHP()}.";
		if(IsCritical()) res = $"{res} Le queda poca vida.";
		if(IsPlayerControlled()) res = $"{res} Maná: {data_Info.giveMP()} de {data_Info.giveMAXMP()}.";
		if(statusController.ThereIsActiveStatus()) res = $"{res} {statusController.CurrentStatusesToString()}.";
		else res = $"{res} Sin efectos de estado.";
		return res;
	}
	
	public Texture2D GiveTexture(){
		// Nombre de la animación que se está reproduciendo
		string anim = sprites.Animation;
		int frame = sprites.Frame;
		// Devuelve la textura del frame actual
		return sprites.SpriteFrames.GetFrameTexture(anim, frame);
	}
	
}
