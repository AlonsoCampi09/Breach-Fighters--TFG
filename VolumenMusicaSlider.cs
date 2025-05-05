using Godot;
using System;

public partial class VolumenMusicaSlider : HSlider
{
    [Export] string bus_name;
    int bus_index;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bus_index = AudioServer.GetBusIndex(bus_name);

        Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(bus_index));
    }



    public void _OnValueChanged(float value)
    {
        AudioServer.SetBusVolumeDb(bus_index, Mathf.LinearToDb(value));
        double value_aux = value / this.MaxValue * 100;
        DisplayServer.TtsStop();
        string mensaje = "Volumen de musica a " + (int)value_aux;
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = mensaje;

            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
}
