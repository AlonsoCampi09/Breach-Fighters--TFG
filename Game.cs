using Godot;
using System;
public partial class Game : Node2D
{
	private FighterTeam allies;
	AudioStreamPlayer2D musica;
	bool musica_bajada = false, music_frist_play = false;
	Label label;
    private PauseMenu MenuPausa;
	Node battle;
    // [Export] PackedScene scenePause;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		allies = GetNode<FighterTeam>("Equipo_Aliado");
		musica = GetNode<AudioStreamPlayer2D>("musicaBatalla");
		label = GetNode<Label>("Label");
		label.Visible = false;
		MenuPausa = GetNode<PauseMenu>("PauseMenu");
        play();
        CustomSignals.Instance.Connect(nameof(CustomSignals.Instance.Pausa), Callable.From(pauseMenu), (uint)GodotObject.ConnectFlags.Deferred);

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
		if(!musica.Playing && music_frist_play)
		{
			musica.Play();
		}

		/*if (Input.IsActionJustPressed("pausa"))
        {
			pauseMenu();	
        }*/
    }

	public async void play()
	{
		int floorLevel = 1, floorRoom = 1;
		var scene = GD.Load<PackedScene>("res://battle.tscn");
        var cinematica = GD.Load<PackedScene>("res://cinematica_inicial.tscn");
        var cinematica_instance = cinematica.Instantiate();
		allies.Visible = false;
		DisplayServer.TtsStop();
		CustomSignals.cinematicaPausa = true;
        AddChild(cinematica_instance);
        await ToSignal(CustomSignals.Instance, "cinematica");
        CustomSignals.cinematicaPausa = false;

        musica.Play();
		music_frist_play = true;
        allies.Visible = true;
        
        allies.ZIndex = 1;
		label.Visible = true;
		label.Text = $"PISO {floorLevel} - HABITACIÓN {floorRoom}";
		GD.Print($"{label.Text}");
        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(label.Text, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
        // Esperar 2 segundos
        await ToSignal(GetTree().CreateTimer(2), "timeout");
		// Ocultar el texto
		label.Visible = false;
		var instance = scene.Instantiate();
        
        AddChild(instance);
		//GetChild(instance.GetIndex(false));
		battle = instance;
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

	public void pauseMenu()
	{
		if (!CustomSignals.pausado)
		{
            DisplayServer.TtsStop();
            MenuPausa.Visible = false;
            allies.Visible = true;
			battle.Call("show");
            Engine.TimeScale = 1;
            GD.Print("pausado = false");

        }
        else
		{
            DisplayServer.TtsStop();

            MenuPausa.Visible = true;
            allies.Visible = false;
            battle.Call("hide");
			GD.Print("pausado = true");
            Engine.TimeScale = 0;
        }
        //CustomSignals.pausado = !CustomSignals.pausado;

    }
}
