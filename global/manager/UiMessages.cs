using Godot;
using System;

public partial class UiMessages : Node{
	
	private CustomSignals customSignals;
	private Label label;
	
	public override void _Ready(){
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.OnShowFighterInAction += FighterInAction;
		customSignals.OnVictory += VictoryMessage;
	}
	
	public void GiveLabel(Label l){
		label = l;
	}
	
	public async void ShowRoomInfo(GameState gameState){
		// Mostrar el texto
		label.Text = $"PISO {gameState.floorLevel} - HABITACIÓN {gameState.floorRoom}";
		TTS.PutThisInQueue($"Habitación {gameState.floorRoom} del piso {gameState.floorLevel}.");
		label.Visible = true;
		// Esperar 2 segundos
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		// Ocultar el texto
		label.Visible = false;
	
		customSignals.EmitSignal(nameof(CustomSignals.OnPrepareRoom));
	}
	
	public async void VictoryMessage(int exp, int coins){
		// Mostrar el texto
		label.Text = $"Victoria! Exp: +{exp} | Coins: +{coins}";
		TTS.SayThis($"Victoria! el equipo gana {exp} de experiencia y {coins} monedas.");
		label.Visible = true;
		// Esperar 2 segundos
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		// Ocultar el texto
		label.Visible = false;
	
		customSignals.EmitSignal(nameof(CustomSignals.OnNextRoom), exp, coins);
	}
	
	public async void ShowDangerAlert(){
		// Mostrar el texto
		label.Text = "PELIGRO";
		label.Visible = true;
		// Esperar 2 segundos
		TTS.PutThisInQueue("¡Se acercan enemigos!");
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		label.Text = "Enemigos acercandose!";
		// Esperar 2 segundos
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		// Ocultar el texto
		label.Visible = false;
		
		customSignals.EmitSignal(nameof(CustomSignals.OnGenerateEnemyTeamForRoom));
	}
	
	
	
	public async void ReadyForTheBattle(){
		// Mostrar el texto
		label.Text = "Preparados?";
		label.Visible = true;
		// Esperar 1 segundos
		await ToSignal(GetTree().CreateTimer(0.5), "timeout");
		label.Text = "Listos?";
		// Esperar 1 segundos
		await ToSignal(GetTree().CreateTimer(0.5), "timeout");
		label.Text = "Al ataque!";
		// Esperar 1 segundos
		TTS.PutThisInQueue("¡Empieza el combate!");
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		// Ocultar el texto
		label.Visible = false;
		customSignals.EmitSignal(nameof(CustomSignals.OnStartTeamBattle));
	}
	
	public async void FighterInAction(Fighter actor){
		// Mostrar el texto
		if(!actor.IsCritical())
			actor.PlayAnimationSafe("acting");
		label.Text = $"Turno de {actor.GetEntityData().Name}";
		TTS.PutThisInQueue($"Le toca a {actor.GetEntityData().Name}");
		label.Visible = true;
		// Esperar 2 segundos
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		// Ocultar el texto
		label.Visible = false;
		if(!actor.IsCritical())
			actor.PlayAnimationSafe("idle");
		customSignals.EmitSignal(nameof(CustomSignals.OnShowBattleMenu));
	}
	
	public async void RestingMessage(){
		// Mostrar el texto
		label.Text = $"Punto de descanso";
		TTS.PutThisInQueue($"Punto de descanso");
		//TTS.PutThisInQueue($"");
		label.Visible = true;
		// Esperar 2 segundos
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		// Ocultar el texto
		label.Visible = false;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowRestHUD));
	}
}
