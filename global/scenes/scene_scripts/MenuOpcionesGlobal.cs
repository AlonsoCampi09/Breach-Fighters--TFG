using Godot;
using System;

public partial class MenuOpcionesGlobal : Control{
	
	private Panel optionMenu;
	private Button volumenButton;
	private Button accesibilityButton;
	private Button keyButton;
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
	private bool abierto = false; 
	private Panel keyMenu;
	private Button key1;
	private Button key2;
	private Button key3;
	private Button key4;
	private Button key5;
	private Button key6;
	private Button key7;
	private Button key8;
	private Label fun1;
	private Label fun2;
	private Label fun3;
	private Label fun4;
	private Label fun5;
	private Label fun6;
	private Label fun7;
	private Label fun8;
	private Label tecla1;
	private Label tecla2;
	private Label tecla3;
	private Label tecla4;
	private Label tecla5;
	private Label tecla6;
	private Label tecla7;
	private Label tecla8;
	
	private Button back4;
	
	AudioStreamPlayer2D sfx;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		sfx = GetNode<AudioStreamPlayer2D>("Efectos");
		optionMenu = GetNode<Panel>("Option_Menu");
		volumenButton = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Volume");
		accesibilityButton = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Accesibility");
		keyButton = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Controls");
		back1 = GetNode<Button>("Option_Menu/MarginContainer/VBoxContainer/VBoxContainer/Back");
		
		volumenButton.Pressed += OpenVolumeMenu;
		accesibilityButton.Pressed += OpenAccesibiltyMenu;
		keyButton.Pressed += OpenKeyMenu;
		back1.Pressed += CloseGlobalOptionMenu;
		
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
		AudioServer.SetBusVolumeDb(0, PercentToDb((float)volumeMaster.Value));
		labelSound.Text = $"{volumeMaster.Value}%";
		AudioServer.SetBusVolumeDb(1, PercentToDb((float)volumeMusic.Value));
		labelSound.Text = $"{volumeSound.Value}%";
		AudioServer.SetBusVolumeDb(2, PercentToDb((float)volumeSound.Value));
		labelSound.Text = $"{volumeSound.Value}%";
		
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
		
		keyMenu = GetNode<Panel>("Controls_Menu");
		key1 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button");
		key2 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button2");
		key3 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button3");
		key4 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button4");
		key5 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button8");
		key6 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button5");
		key7 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button6");
		key8 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button7");
		fun1 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button/HBoxContainer/Label");
		fun2 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button2/HBoxContainer2/Label");
		fun3 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button3/HBoxContainer3/Label");
		fun4 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button4/HBoxContainer4/Label");
		fun5 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button8/HBoxContainer4/Label");
		fun6 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button5/HBoxContainer5/Label");
		fun7 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button6/HBoxContainer6/Label");
		fun8 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button7/HBoxContainer7/Label");
		tecla1 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button/HBoxContainer/HBoxContainer/Label");
		tecla2 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button2/HBoxContainer2/HBoxContainer/Label");
		tecla3 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button3/HBoxContainer3/HBoxContainer/Label");
		tecla4 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button4/HBoxContainer4/HBoxContainer/Label");
		tecla5 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button8/HBoxContainer4/HBoxContainer/Label");
		tecla6 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button5/HBoxContainer5/HBoxContainer/Label");
		tecla7 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button6/HBoxContainer6/HBoxContainer/Label");
		tecla8 = GetNode<Label>("Controls_Menu/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer/Button7/HBoxContainer7/HBoxContainer/Label");
		back4 = GetNode<Button>("Controls_Menu/MarginContainer/VBoxContainer/BackControls");
		back4.Pressed += OpenOptionMenu;
		
		ttsVolume.Value = 50;
		ttsVelocity.Value = 1.5;
		
		TTS.SetVolume((float)ttsVolume.Value);
		labelTTSVolume.Text = $"{ttsVolume.Value}%";
		TTS.SetSpeed((float)ttsVelocity.Value);
		labelVelocity.Text = $"{ttsVelocity.Value}";
		
		volumenButton.FocusEntered += OnFocusEnteredVolume;
		accesibilityButton.FocusEntered += OnFocusEnteredAccesibility;
		keyButton.FocusEntered += OnFocusEnteredControls;
		back1.FocusEntered += OnFocusEnteredBack;
		volumeMaster.FocusEntered += OnFocusEnteredMainSlider;
		volumeMusic.FocusEntered += OnFocusEnteredMusicSlider;
		volumeSound.FocusEntered += OnFocusEnteredSFXSlider;
		back2.FocusEntered += OnFocusEnteredBack;
		ttsSwitch.FocusEntered += OnFocusEnteredCheckBox;
		ttsVelocity.FocusEntered += OnFocusEnteredTTSVelSlider;
		ttsVolume.FocusEntered += OnFocusEnteredTTSVolumeSlider;
		back3.FocusEntered += OnFocusEnteredBack;
		key1.FocusEntered += OnFocusEnteredKey1;
		key2.FocusEntered += OnFocusEnteredKey2;
		key3.FocusEntered += OnFocusEnteredKey3;
		key4.FocusEntered += OnFocusEnteredKey4;
		key5.FocusEntered += OnFocusEnteredKey5;
		key6.FocusEntered += OnFocusEnteredKey6;
		key7.FocusEntered += OnFocusEnteredKey7;
		key8.FocusEntered += OnFocusEnteredKey8;
		back4.FocusEntered += OnFocusEnteredBack; 
		
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		keyMenu.Visible = false;
		
	}

    private void OpenOptionMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = true;
		accesibilityMenu.Visible = false;
		keyMenu.Visible = false;
		volumenButton.GrabFocus();
	}
	private void OpenVolumeMenu(){
		volumeMenu.Visible = true;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		keyMenu.Visible = false;
		volumeMaster.GrabFocus();
	}
	private void OpenAccesibiltyMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = true;
		keyMenu.Visible = false;
		ttsSwitch.GrabFocus();
	}
	private void OpenKeyMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		keyMenu.Visible = true;
		key1.GrabFocus();
	}
	private void CloseGlobalOptionMenu(){
		volumeMenu.Visible = false;
		optionMenu.Visible = false;
		accesibilityMenu.Visible = false;
		keyMenu.Visible = false;
	}
	
	private void OnFocusEnteredVolume(){
		TTS.SayThis(volumenButton.Text);
	}
	private void OnFocusEnteredAccesibility(){
		TTS.SayThis(accesibilityButton.Text);
	}
	private void OnFocusEnteredControls(){
		TTS.SayThis(keyButton.Text);
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
	private void OnFocusEnteredKey1(){
		TTS.SayThis($"{fun1.Text}. Tecla {tecla1.Text}");
	}
	private void OnFocusEnteredKey2(){
		TTS.SayThis($"{fun2.Text}. Tecla {tecla2.Text}");
	}
	private void OnFocusEnteredKey3(){
		TTS.SayThis($"{fun3.Text}. Tecla {tecla3.Text}");
	}
	private void OnFocusEnteredKey4(){
		TTS.SayThis($"{fun4.Text}. Tecla {tecla4.Text}");
	}
	private void OnFocusEnteredKey5(){
		TTS.SayThis($"{fun5.Text}. Tecla {tecla5.Text}");
	}
	private void OnFocusEnteredKey6(){
		TTS.SayThis($"{fun6.Text}. Tecla {tecla6.Text}");
	}
	private void OnFocusEnteredKey7(){
		TTS.SayThis($"{fun7.Text}. Tecla {tecla7.Text}");
	}
	private void OnFocusEnteredKey8(){
		TTS.SayThis($"{fun8.Text}. Tecla {tecla8.Text}");
	}
	
	
	private void OnMasterVolumeChanged(double value){
		AudioServer.SetBusVolumeDb(0, PercentToDb((float)value));
		ConfigData.masterVolumeValue = (float)value;
		labelMaster.Text = $"{value}%";
	}
	private void OnMusicVolumeChanged(double value){
		AudioServer.SetBusVolumeDb(1, PercentToDb((float)value));
		ConfigData.musicVolumeValue = (float)value;
		labelMusic.Text = $"{value}%";
	}
	private void OnSFXVolumeChanged(double value){
		AudioServer.SetBusVolumeDb(2, PercentToDb((float)value));
		ConfigData.sfxVolumeValue = (float)value;
		labelSound.Text = $"{value}%";
		sfx.Play();
	}
	

	private void OnVisibilityChanged()
	{
		if (!abierto)
		{
			GD.Print("Abriendo menu...");
			OpenOptionMenu();
		}
		else {
            GD.Print("Cerrando menu...");
			//CloseGlobalOptionMenu(); 
        } 
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
