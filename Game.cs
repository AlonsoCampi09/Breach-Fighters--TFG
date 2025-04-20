using Godot;
using System;

public partial class Game : Node2D
{
    private FighterTeam allies;
    AudioStreamPlayer2D musica;
    bool musica_bajada = false;
    //private PackedScene[] batallas;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        allies = GetNode<FighterTeam>("Equipo_Aliado");
        musica = GetNode<AudioStreamPlayer2D>("musicaBatalla");
        musica.Play();
        
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
        var scene = GD.Load<PackedScene>("res://battle.tscn");
        var instance = scene.Instantiate();
        AddChild(instance);
        instance.Call("Play_Batalla");
        while (true) {
            await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.Battlefinished));
            instance.QueueFree();
            instance = scene.Instantiate();
            AddChild(instance);
            instance.Call("Play_Batalla");
        }
    }
}
