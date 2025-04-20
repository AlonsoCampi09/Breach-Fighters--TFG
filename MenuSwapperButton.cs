using Godot;
using System;

public partial class MenuSwapperButton : Button
{
	[Export] Node SwitchToMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnMenuSwapperButtonPressed;

    }

    private void OnMenuSwapperButtonPressed()
    {
		if(GetParent().GetParent() is MenuTab menuTab)
		{

			menuTab.OnMenuSwapButtonPressed(SwitchToMenu.GetIndex());
		}
    }


}
