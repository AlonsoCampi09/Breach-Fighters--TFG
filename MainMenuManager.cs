using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainMenuManager : Control
{
    List<int> goBackList = new();
    Button start, opciones, salir, retu;
    HSlider SoundCues, Tts, MusicaSlider;
    AudioStreamPlayer2D musica;
    bool musica_bajada = false;
    CheckBox TTScheck;
    public override void _Ready()
    {
        start = GetNode<Button>("MenuTab/VBoxContainer/Start Game");
        opciones = GetNode<Button>("MenuTab/VBoxContainer/OptionsButton");
        salir = GetNode<Button>("MenuTab/VBoxContainer/QuitButton");
        SoundCues = GetNode<HSlider>("MenuOpciones/VBoxContainer/SoundCueSlider");
        Tts = GetNode<HSlider>("MenuOpciones/VBoxContainer/TextToSpeachSlider");
        MusicaSlider = GetNode<HSlider>("MenuOpciones/VBoxContainer/VolumenMusicaSlider");
        retu = GetNode<Button>("MenuOpciones/VBoxContainer/ReturnButton");
        musica = GetNode<AudioStreamPlayer2D>("musicaMenu");
        musica.Play();
        TTScheck = GetNode<CheckBox>("MenuOpciones/VBoxContainer/CheckBox");
        TTScheck.Pressed += TTScheckbutton;
        start.GrabFocus();

    }

    public override void _Process(double delta)
    {

        if (DisplayServer.TtsIsSpeaking() && !musica_bajada)
        {
            musica.VolumeDb = musica.VolumeDb - 20;
            GD.Print("musica bajada");

            GD.Print(musica.VolumeDb);
            musica_bajada = true;
        }

        if(!DisplayServer.TtsIsSpeaking() && musica_bajada)
        {
            musica.VolumeDb = musica.VolumeDb + 20;
            GD.Print("musica subida");

            GD.Print(musica.VolumeDb);

            musica_bajada = false;
        }
        if (!musica.Playing)
        {
            musica.Play();
        }

    }
    private void _OnStartFocusEntered()
    {

        DisplayServer.TtsStop();

        string Message = start.Text;
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }

    private void CambioOpciones()
    {
        SoundCues.GrabFocus();
        musica.Stop();

    }
    private void CambioPrincipal()
    {
        start.GrabFocus();
        musica.Play();

    }

    private void TTScheckbutton()
    {
        GD.Print("TTS cambio");

        if (!CustomSignals.activado)
        {
            CustomSignals.activado = true;
        }
        else
        {
            CustomSignals.activado = false;
        }
    }

    private void _OnSoundCuesFocusEntered()
    {
        DisplayServer.TtsStop();
        string Message = "volumen de Saound Cues";
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnMusicaSliderFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = "volumen de la musica";
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnTtsFocusEntered()
    {
        DisplayServer.TtsStop();
        string Message = "volumen de Text to Speech";
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnTtsVelocidadFocusEntered()
    {
        DisplayServer.TtsStop();
        string Message = "Velocidad de Text to Speech";
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnRetunrButtonFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = retu.Text;
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnOpcionesFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = opciones.Text;
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnSalirFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = salir.Text;
        if (CustomSignals.activado)
        {CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
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
