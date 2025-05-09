using Godot;
using System;

public partial class Game : Node2D
{
	private FighterTeam allies;
	AudioStreamPlayer2D musica;
	bool musica_bajada = false;
	Label label;
	//private PackedScene[] batallas;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		allies = GetNode<FighterTeam>("Equipo_Aliado");
		musica = GetNode<AudioStreamPlayer2D>("musicaBatalla");
		label = GetNode<Label>("Label");
		musica.Play();
		label.Visible = false;
		play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (DisplayServer.TtsIsSpeaking() && !musica_bajada)
		{
			musica.VolumeDb = musica.VolumeDb - 20;
			GD.Print("musica bajada");

			GD.Print(musica.VolumeDb);
			musica_bajada = true;
		}

		if (!DisplayServer.TtsIsSpeaking() && musica_bajada)
		{
			musica.VolumeDb = musica.VolumeDb + 20;
			GD.Print("musica subida");

			GD.Print(musica.VolumeDb);

			musica_bajada = false;
		}
		if(!musica.Playing)
		{
			musica.Play();
		}
	}

	public async void play()
	{
		int floorLevel = 1, floorRoom = 1;
		var scene = GD.Load<PackedScene>("res://battle.tscn");
        var cinematica = GD.Load<PackedScene>("res://cinematica_inicial.tscn");
        var cinematica_instance = cinematica.Instantiate();
		allies.Visible = false;
        AddChild(cinematica_instance);
     //   cinematica_instance.Call("Escena");
        await ToSignal(CustomSignals.Instance, "cinematica");
        allies.Visible = true;
        
        allies.ZIndex = 1;
		label.Visible = true;
		label.Text = $"PISO {floorLevel} - HABITACIÓN {floorRoom}";
		GD.Print($"{label.Text}");
		// Esperar 2 segundos
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		// Ocultar el texto
		label.Visible = false;
		var instance = scene.Instantiate();
		AddChild(instance);
		instance.Call("Play_Batalla");
		while (true) {
			await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.Battlefinished));
			label.Text = $"PISO {floorLevel} - HABITACIÓN {floorRoom}";
			label.Visible = true;
			// Esperar 2 segundos
			await ToSignal(GetTree().CreateTimer(2), "timeout");
			// Ocultar el texto
			label.Visible = false;
			instance.QueueFree();
			instance = scene.Instantiate();
			AddChild(instance);
			instance.Call("Play_Batalla");
		}
	}
}
