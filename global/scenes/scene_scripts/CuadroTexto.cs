using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CuadroTexto : Control{
	private CustomSignals customSignals;

	private float characterDelay = 0.01f;
	private string fullText = "";
	private Label label;
	private Panel panel;
	private int currentCharIndex = 0;
	private float timeAccumulator = 0f;
	
	private AudioStreamPlayer2D typeSound;

	private bool finishedTyping = false;
	private bool isWriting = false;
	
	private bool inUse = false;

	private Queue<string> messageQueue = new Queue<string>();

	public override void _Ready(){
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.OnShowDialog += QueueDialog; // CAMBIO: ahora cola
		panel = GetNode<Panel>("Panel");
		label = GetNode<Label>("Panel/MarginContainer/Label");
		typeSound = GetNode<AudioStreamPlayer2D>("TypeSound");
		label.Text = "";
		this.Visible = false;
	}

	public override void _Process(double delta){
		if (isWriting && currentCharIndex < fullText.Length){
			typeSound.Play();
			timeAccumulator += (float)delta;
			if (timeAccumulator >= characterDelay){
				timeAccumulator = 0f;
				currentCharIndex++;
				label.Text = fullText.Substring(0, currentCharIndex);
				if (currentCharIndex == fullText.Length){
					finishedTyping = true;
				}
			}
		}
		if(Input.IsActionJustPressed("accept") && inUse)
			NextStep();
	}

	private void NextStep(){
		if (!finishedTyping){
			// Si aún se está escribiendo, salta al final
			currentCharIndex = fullText.Length;
			label.Text = fullText;
			finishedTyping = true;
			return;
		}
		// Si ya acabó el texto actual, mostrar siguiente
		if (messageQueue.Count > 0){
			ShowDialog(messageQueue.Dequeue());
		} else {
			this.Visible = false;
			isWriting = false;
			inUse = false;
			customSignals.EmitSignal(nameof(CustomSignals.OnDialogIsOver));
		}
	}

	// NUEVO: Añadir a la cola
	public void QueueDialog(string text){
		messageQueue.Enqueue(text);
		if (!isWriting){
			ShowDialog(messageQueue.Dequeue());
		}
	}

	// Mostrador de texto
	private void ShowDialog(string text){
		fullText = text;
		currentCharIndex = 0;
		timeAccumulator = 0f;
		isWriting = true;
		label.Text = "";
		this.Visible = true;
		finishedTyping = false;
		TTS.PutThisInQueue(text);
		inUse = true;
	}
	
	public override void _ExitTree(){
		customSignals.OnShowDialog -= QueueDialog; // o ShowDialog si es el que usas
	}
}
