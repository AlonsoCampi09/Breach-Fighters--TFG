using Godot;
using System;

public partial class TTSVelocidadSlider : HSlider
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Value = CustomSignals.velocidadTextToSpeach;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnValueChanged(float value)
	{

		CustomSignals.velocidadTextToSpeach = (int)value;
	   // double value_aux = (int)value / this.MaxValue * 100;
		DisplayServer.TtsStop();
		string mensaje = "Velocidad de Text To Speech a " + value;
		if (CustomSignals.activado)
		{CustomSignals.Instance.repetir = mensaje;

			DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
		}
	}
}
