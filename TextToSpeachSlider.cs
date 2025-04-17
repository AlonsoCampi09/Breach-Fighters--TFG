using Godot;
using System;

public partial class TextToSpeachSlider : HSlider
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Value = CustomSignals.volumenTextToSpeach;

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void _OnValueChanged(float value)
    {

        CustomSignals.volumenTextToSpeach = (int)value;
    }
}
