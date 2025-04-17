using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainMenuManager : Control
{
    List<int> goBackList = new();

    public void SwapMenu(int menuIndex, int returnIndex)
    {
        if(GetChild(menuIndex) is MenuTab menutab)
        {
            menutab.Visible = true;
        }

        if (returnIndex < 0) return;
        goBackList.Add(returnIndex);
    }

    public void SwapMenuToPrevius()
    {
        if (!goBackList.Any()) return;
        SwapMenu(goBackList[goBackList.Count - 1], -1); // al ser -1 no se anade en goBacklist 
        goBackList.RemoveAt(goBackList.Count - 1);
    }

    public void OnSwapScene(PackedScene loadScene)
    {
        GetTree().Root.AddChild(loadScene.Instantiate());
        QueueFree(); //se borra este nodo con sus hijos
    }

    private void OnQuitGameButtonPressed()
    {
        GetTree().Quit();
    }
}
