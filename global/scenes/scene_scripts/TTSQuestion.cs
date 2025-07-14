using Godot;
using System;

public partial class TTSQuestion : CanvasLayer{
	
	private int estado = 0;
	private Panel panelReady;
	
	private VBoxContainer container;
	private VBoxContainer question;
	private HSlider ttsVelocity;
	private Label labelVelocity;
	private HSlider ttsVolume;
	private Label labelTTSVolume;
	private Button ready;
	
	public override void _Ready(){
		question = GetNode<VBoxContainer>("TTSQuestion");
		container = GetNode<VBoxContainer>("VBoxContainer");
		ttsVelocity = GetNode<HSlider>("VBoxContainer/HBoxContainer2/HBoxContainer/HSlider");
		labelVelocity = GetNode<Label>("VBoxContainer/HBoxContainer2/HBoxContainer/Label");
		ttsVolume = GetNode<HSlider>("VBoxContainer/HBoxContainer3/HBoxContainer/HSlider");
		labelTTSVolume = GetNode<Label>("VBoxContainer/HBoxContainer3/HBoxContainer/Label");
		ready = GetNode<Button>("VBoxContainer/Ready");
		
		panelReady = GetNode<Panel>("Panel");
		
		ttsVolume.Value = ConfigData.ttsVolumeValue;
		ttsVelocity.Value = ConfigData.ttsVelocityValue;
		
		TTS.SetVolume((float)ttsVolume.Value);
		labelTTSVolume.Text = $"{ttsVolume.Value}%";
		TTS.SetSpeed((float)ttsVelocity.Value);
		labelVelocity.Text = $"{ttsVelocity.Value}";
		
		ttsVelocity.FocusEntered += OnFocusEnteredTTSVelSlider;
		ttsVolume.FocusEntered += OnFocusEnteredTTSVolumeSlider;
		ready.FocusEntered += OnFocusEnteredReady;
		
		ready.Pressed += OnReadyPressed;
		
		question.Visible = true;
		panelReady.Visible = false;
		container.Visible = false;
		
		ttsVolume.Value = ConfigData.ttsVolumeValue;
		ttsVelocity.Value = ConfigData.ttsVelocityValue;
		
		ttsVelocity.ValueChanged += OnTTSSpeedChanged;
		ttsVolume.ValueChanged += OnTTSVolumeChanged;
		
		TTS.SetVolume((float)ttsVolume.Value);
		labelTTSVolume.Text = $"{ttsVolume.Value}%";
		TTS.SetSpeed((float)ttsVelocity.Value);
		labelVelocity.Text = $"{ttsVelocity.Value}";
		
		TTS.SayThis($"Voz a texto está disponible y activado.");
		TTS.PutThisInQueue($"Presiona el espacio para manternerme activado.");
		TTS.PutThisInQueue($"Presiona la tecla x para desactivarme.");
		TTS.PutThisInQueue($"Podrás configurarlo luego en el menú de accesibilidad de las opciones.");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if (Input.IsActionJustPressed("ui_accept") && estado == 0){
			estado = 1;
			OpenTTSConfiguration();
		}
		else if (Input.IsActionJustPressed("ui_cancel") && estado == 0){
			TTS.StopTTS();
			ConfigData.ttsValue = false;
			TTS.EnableDisableTTS(ConfigData.ttsValue);
			GetTree().ChangeSceneToFile("res://global/scenes/scenesUI/menu_main.tscn");
		}
		else if (Input.IsActionJustPressed("ui_cancel") && estado == 2){
			ClosePanel();
		}
		else if (Input.IsActionJustPressed("ui_accept") && estado == 2){
			TTS.StopTTS();
			GetTree().ChangeSceneToFile("res://global/scenes/scenesUI/menu_main.tscn");
		}
	}
	
	private void OpenTTSConfiguration(){
		container.Visible = true;
		question.Visible = false;
		TTS.SayThis($"Muy bien. Vamos a configurarme.");
		ttsVelocity.GrabFocus();
	}
	
	private void OpenPanel(){
		panelReady.Visible = true;
		question.Visible = false;
		TTS.SayThis($"Estos son los cambios que deseas?");
		TTS.PutThisInQueue($"Presiona el espacio para confirmar.");
		TTS.PutThisInQueue($"Presiona la tecla x para cancelar y seguir configurandome.");
	}
	private void ClosePanel(){
		estado = 1;
		panelReady.Visible = false;
	}
	
	private void OnReadyPressed(){
		estado = 2;
		OpenPanel();
	}
	
	private void OnFocusEnteredReady(){
		TTS.SayThis($"Listo");
	}
	private void OnFocusEnteredTTSVelSlider(){
		TTS.PutThisInQueue($"Velocidad texto a voz. Nivel {labelVelocity.Text}");
	}
	private void OnFocusEnteredTTSVolumeSlider(){
		TTS.PutThisInQueue($"Volumen de texto a voz. {labelTTSVolume.Text}");
	}
	
	private void OnTTSVolumeChanged(double value){
		TTS.SetVolume((float)value);
		TTS.StopTTS();
		ConfigData.ttsVolumeValue = (float)value;
		TTS.SayThis("Hola? Probando.");
		labelTTSVolume.Text = $"{value}%";
	}
	private void OnTTSSpeedChanged(double value){
		TTS.SetSpeed((float)value);
		TTS.StopTTS();
		ConfigData.ttsVelocityValue = (float)value;
		TTS.SayThis("Probando. Probando.");
		labelVelocity.Text = $"{value}";
	}
}
