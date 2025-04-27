using Godot;
using System;


public partial class CuadroTexto : Control
{
	private CustomSignals customSignals;
	
	private float characterDelay = 0.01f; // Tiempo entre letras
	private string fullText = "";
	private Label label;
	private Panel panel;
	private Button boton;
	private AudioStreamPlayer2D typeSound;
	private int currentCharIndex = 0;
	private float timeAccumulator = 0f;
	
	private bool finishedTyping = false;
	private bool isWriting = false;

	public override void _Ready()
	{
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.OnDialogRequested += ShowDialog;
		typeSound = GetNode<AudioStreamPlayer2D>("TypeSound");
		panel = GetNode<Panel>("Panel");
		label = GetNode<Label>("Panel/MarginContainer/Label");
		boton = GetNode<Button>("Panel/MarginContainer/Label/Button");
		label.Text = "";
		panel.Visible = false;
		boton.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (isWriting && currentCharIndex < fullText.Length)
		{
			timeAccumulator += (float)delta;
			if (timeAccumulator >= characterDelay)
			{
				timeAccumulator = 0f;
				currentCharIndex++;
				label.Text = fullText.Substring(0, currentCharIndex);
				 if (!finishedTyping)
					typeSound.Play();
				if (currentCharIndex == fullText.Length){
					finishedTyping = true;
					boton.Visible = true;
					boton.GrabFocus();
				}
			}
		}
	}
	public void OnButtonPressed(){
		GD.Print("Boton dado.");
		panel.Visible = false;
		customSignals.EmitSignal(nameof(CustomSignals.OnDialogConfirmed));
		
	}
	
	public void ShowDialog(string text)
	{
        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(text, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
        fullText = text;
		currentCharIndex = 0;
		timeAccumulator = 0f;
		isWriting = true;
		label.Text = "";
		panel.Visible = true;
		boton.Visible = false;
		finishedTyping = false;
	}
}
