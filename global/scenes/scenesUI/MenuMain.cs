using Godot;
using System;

public partial class MenuMain : Control{
	
	private VBoxContainer main;
	private Button start;
	private Button options;
	private Button exit;
	
	private Panel optionMenu;
	private Button volumenButton;
	private Button accesibilityButton;
	private Button back1;
	
	private Panel volumeMenu;
	private HSlider volumeMaster;
	private Label labelMaster;
	private HSlider volumeMusic;
	private Label labelMusic;
	private HSlider volumeSound;
	private Label labelSound;
	private Button back2;
	
	private Panel accesibilityMenu;
	private Label labelTTS;
	private CheckBox ttsSwitch;
	private HSlider ttsVelocity;
	private Label labelVelocity;
	private HSlider ttsVolume;
	private Label labelTTSVolume;
	private Button back3;
	private bool tts = true;
	
	AudioStreamPlayer2D musica;
	AudioStreamPlayer2D sfx;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		musica = GetNode<AudioStreamPlayer2D>("Musica");
		sfx = GetNode<AudioStreamPlayer2D>("Efectos");
		main = GetNode<VBoxContainer>("Main");
		start = GetNode<Button>("Main/Start");
		options = GetNode<Button>("Main/Options");
		exit = GetNode<Button>("Main/Exit");
		
		start.Pressed += OnStartGame;
		options.Pressed += OpenOptionMenu; 
		exit.Pressed += OnExitGame;
		
		optionMenu = GetNode<Panel>("Option_Menu");
		volumenButton = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Volume");
		accesibilityButton = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Accesibility");
		back1 = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Back");
		
		volumenButton.Pressed += OpenVolumeMenu;
		accesibilityButton.Pressed += OpenAccesibiltyMenu;
		back1.Pressed += OpenMainMenu;
		
		volumeMenu = GetNode<Panel>("Volume_Menu");
		volumeMaster = GetNode<HSlider>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/HBoxContainer/HBoxContainer/HSlider");
		labelMaster = GetNode<Label>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/HBoxContainer/HBoxContainer/Label");
		volumeMusic = GetNode<HSlider>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/HBoxContainer2/HBoxContainer/HSlider");
		labelMusic = GetNode<Label>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/HBoxContainer2/HBoxContainer/Label");
		volumeSound = GetNode<HSlider>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/HBoxContainer3/HBoxContainer/HSlider");
		labelSound = GetNode<Label>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/HBoxContainer3/HBoxContainer/Label");
		back2 = GetNode<Button>("Volume_Menu/MarginContainer/VBoxContainer/VBoxContainer/BackVolume");
		
		volumeMaster.ValueChanged += OnMasterVolumeChanged;
		volumeMusic.ValueChanged += OnMusicVolumeChanged;
		volumeSound.ValueChanged += OnSFXVolumeChanged;
		back2.Pressed += OpenOptionMenu;
		
		volumeMaster.Value = 100; // Master
		volumeMusic.Value = 100;  // Music
		volumeSound.Value = 100;  // SFX
		OnMasterVolumeChanged(volumeMaster.Value);
		OnMusicVolumeChanged(volumeMusic.Value);
		OnSFXVolumeChanged(volumeSound.Value);
		
		accesibilityMenu = GetNode<Panel>("Accesibility_Menu");
		labelTTS = GetNode<Label>("Accesibility_Menu/MarginContainer/VBoxContainer/HBoxContainer/Label");
		ttsSwitch = GetNode<CheckBox>("Accesibility_Menu/MarginContainer/VBoxContainer/HBoxContainer/CheckBox");
		ttsVelocity = GetNode<HSlider>("Accesibility_Menu/MarginContainer/VBoxContainer/HBoxContainer2/HBoxContainer/HSlider");
		labelVelocity = GetNode<Label>("Accesibility_Menu/MarginContainer/VBoxContainer/HBoxContainer2/HBoxContainer/Label");
		ttsVolume = GetNode<HSlider>("Accesibility_Menu/MarginContainer/VBoxContainer/HBoxContainer3/HBoxContainer/HSlider");
		labelTTSVolume = GetNode<Label>("Accesibility_Menu/MarginContainer/VBoxContainer/HBoxContainer3/HBoxContainer/Label");
		back3 = GetNode<Button>("Accesibility_Menu/MarginContainer/VBoxContainer/BackAccesibility");
		
		ttsVelocity.ValueChanged += OnTTSSpeedChanged;
		ttsVolume.ValueChanged += OnTTSVolumeChanged;
		back3.Pressed += OpenOptionMenu;
		
		ttsVolume.Value = 50;
		ttsVelocity.Value = 1.5;
		
		OnTTSVolumeChanged(ttsVolume.Value);
		OnTTSSpeedChanged(ttsVelocity.Value);
		
		start.FocusEntered += OnFocusEnteredStart;
		options.FocusEntered += OnFocusEnteredOptions;
		exit.FocusEntered += OnFocusEnteredExit;
		volumenButton.FocusEntered += OnFocusEnteredVolume;
		accesibilityButton.FocusEntered += OnFocusEnteredAccesibility;
		back1.FocusEntered += OnFocusEnteredBack;
		volumeMaster.FocusEntered += OnFocusEnteredMainSlider;
		volumeMusic.FocusEntered += OnFocusEnteredMusicSlider;
		volumeSound.FocusEntered += OnFocusEnteredSFXSlider;
		back2.FocusEntered += OnFocusEnteredBack;
		ttsSwitch.FocusEntered += OnFocusEnteredCheckBox;
		ttsVelocity.FocusEntered += OnFocusEnteredTTSVelSlider;
		ttsVolume.FocusEntered += OnFocusEnteredTTSVolumeSlider;
		back3.FocusEntered += OnFocusEnteredBack;
		
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		main.Visible = true;
		
		start.GrabFocus();
		TTS.PutThisInQueue("En el menú principal aparece el logo y el título del juego a la izquierda. A la derecha de la pantalla, una figura elegante misteriosa con una sonrisa y mirada oculta por su sombrero de copa se encuentra sentada en un trono blanco.");
	}
	
	private void OnStartGame(){
		GD.Print("Iniciando el juego...");
		TTS.StopTTS();
		GetTree().ChangeSceneToFile("res://global/scenes/game.tscn");
	}
	private void OnExitGame(){
		GD.Print("Saliendo del juego...");
		GetTree().Quit();
	}
	
	private void OpenMainMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		main.Visible = true;
		start.GrabFocus();
	}
	private void OpenOptionMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = true;
		accesibilityMenu.Visible = false;
		main.Visible = false;
		volumenButton.GrabFocus();
	}
	private void OpenVolumeMenu(){
		volumeMenu.Visible = true;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		main.Visible = false;
		volumeMaster.GrabFocus();
	}
	private void OpenAccesibiltyMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = true;
		main.Visible = false;
		ttsSwitch.GrabFocus();
	}
	
	private void OnFocusEnteredStart(){
		TTS.SayThis(start.Text);
	}
	private void OnFocusEnteredOptions(){
		TTS.SayThis(options.Text);
	}
	private void OnFocusEnteredExit(){
		TTS.SayThis(exit.Text);
	}
	private void OnFocusEnteredVolume(){
		TTS.SayThis(volumenButton.Text);
	}
	private void OnFocusEnteredAccesibility(){
		TTS.SayThis(accesibilityButton.Text);
	}
	private void OnFocusEnteredBack(){
		TTS.SayThis("Volver");
	}
	private void OnFocusEnteredMainSlider(){
		TTS.SayThis($"Volumen general. {labelMaster.Text}");
	}
	private void OnFocusEnteredMusicSlider(){
		TTS.SayThis($"Volumen música. {labelMusic.Text}");
	}
	private void OnFocusEnteredSFXSlider(){
		TTS.SayThis($"Volumen sonidos. {labelSound.Text}");
	}
	private void OnFocusEnteredCheckBox(){
		TTS.SayThis(labelTTS.Text);
	}
	private void OnFocusEnteredTTSVelSlider(){
		TTS.SayThis($"Velocidad texto a voz. Nivel {labelVelocity.Text}");
	}
	private void OnFocusEnteredTTSVolumeSlider(){
		TTS.SayThis($"Volumen de texto a voz. {labelTTSVolume.Text}");
	}
	
	
	private void OnMasterVolumeChanged(double value){
		AudioServer.SetBusVolumeDb(0, PercentToDb((float)value));
		labelMaster.Text = $"{value}%";
	}
	private void OnMusicVolumeChanged(double value){
		AudioServer.SetBusVolumeDb(1, PercentToDb((float)value));
		labelMusic.Text = $"{value}%";
	}
	private void OnSFXVolumeChanged(double value){
		AudioServer.SetBusVolumeDb(2, PercentToDb((float)value));
		labelSound.Text = $"{value}%";
		sfx.Play();
	}
	
	private float PercentToDb(float percent){
		if (percent <= 0f)
			return -80f; // Silencio total
		return Mathf.Lerp(-30f, 0f, percent / 100f);
	}

	private float DbToPercent(float db){
		if (db <= -80f)
			return 0f;
		return Mathf.InverseLerp(-30f, 0f, db) * 100f;
	}
	
	private void OnCheckBoxPressed(){
		if(tts){
			tts = false;
			TTS.EnableDisableTTS(false);
		}
		else{
			tts = true;
			TTS.EnableDisableTTS(true);
		}
	}
	
	private void OnTTSVolumeChanged(double value){
		TTS.SetVolume((float)value);
		TTS.StopTTS();
		TTS.SayThis("Hola? Hola? Probando. No se escucha muy alto, ¿verdad?");
		labelTTSVolume.Text = $"{value}%";
	}
	private void OnTTSSpeedChanged(double value){
		TTS.SetSpeed((float)value);
		TTS.StopTTS();
		TTS.SayThis("Probando. Probando.");
		labelVelocity.Text = $"{value}";
	}
}
