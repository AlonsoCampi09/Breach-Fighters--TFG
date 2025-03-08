using Godot;
using System;

public partial class Game : Node
{

    Node2D logic;
    Control UI;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        base._Ready();

        logic = GetNode<Node2D>("Logica");
        UI = GetNode<Control>("Control");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}
