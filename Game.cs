using Godot;
using System;

public partial class Game : Node2D
{
    private FighterTeam allies;

    //private PackedScene[] batallas;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        allies = GetNode<FighterTeam>("Equipo_Aliado");
        play();

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
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
