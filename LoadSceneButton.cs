using Godot;
using System;

public partial class LoadSceneButton : Button
{
	[Export] PackedScene sceneToSwitchTo;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnSwitchSceneButtonPressed;
	}

	private void OnSwitchSceneButtonPressed()
	{
		if (GetParent().GetParent() is MenuTab menuTab)
		{
			menuTab.LoadSceneRequest(sceneToSwitchTo);
		}
	}
}
