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
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);

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
    private void _OnSoundCuesFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = "volumen de Saound Cues";
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
    }
    private void _OnMusicaSliderFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = "volumen de la musica";
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
    }
    private void _OnTtsFocusEntered()
    {
        DisplayServer.TtsStop();
        string Message = "volumen de Text to Speach";
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
    }
    private void _OnRetunrButtonFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = retu.Text;
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
    }
    private void _OnOpcionesFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = opciones.Text;
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);

    }
    private void _OnSalirFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = salir.Text;
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);

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
