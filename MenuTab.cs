using Godot;
using System;

public partial class MenuTab : PanelContainer
{
    private MainMenuManager mainMenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if(GetParent() is MainMenuManager) {
            mainMenu = GetParent() as MainMenuManager;
        }
	}

    public void OnMenuSwapButtonPressed(int swapIndex)
    {
        mainMenu.SwapMenu(swapIndex, GetIndex());
        Visible = false;
    }

    public void OnMeneReturnButtonPressed()
    {
        mainMenu.SwapMenuToPrevius();
        Visible = false;

    }

    public void LoadSceneRequest(PackedScene loadScene)
    {
        mainMenu.OnSwapScene(loadScene);
    }
}
