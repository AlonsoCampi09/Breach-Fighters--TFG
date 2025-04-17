using Godot;
using System;

public partial class SoundCues : HSlider
{
	[Export] string bus_name;
    int bus_index;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		bus_index = AudioServer.GetBusIndex(bus_name);

		Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(bus_index));
    }



	public void _OnValueChanged(float value) {
		AudioServer.SetBusVolumeDb(bus_index, Mathf.LinearToDb(value));
	}
}
