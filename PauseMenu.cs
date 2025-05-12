using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PauseMenu : Control
{
    List<int> goBackList = new();
    Button resume, opciones, salir, retu;
    HSlider SoundCues, Tts, MusicaSlider;
    AudioStreamPlayer2D musica;
    bool musica_bajada = false;
    CheckBox TTScheck;
    PanelContainer principal, MenuOpciones;
    public override void _Ready()
    {
        resume = GetNode<Button>("MenuTab/VBoxContainer/Resume");
        opciones = GetNode<Button>("MenuTab/VBoxContainer/OptionsButton");
        salir = GetNode<Button>("MenuTab/VBoxContainer/QuitButton");
        SoundCues = GetNode<HSlider>("MenuOpciones/VBoxContainer/SoundCueSlider");
        Tts = GetNode<HSlider>("MenuOpciones/VBoxContainer/TextToSpeachSlider");
        MusicaSlider = GetNode<HSlider>("MenuOpciones/VBoxContainer/VolumenMusicaSlider");
        retu = GetNode<Button>("MenuOpciones/VBoxContainer/ReturnButton");
        TTScheck = GetNode<CheckBox>("MenuOpciones/VBoxContainer/CheckBox");
        principal = GetNode<PanelContainer>("MenuTab");
        MenuOpciones = GetNode<PanelContainer>("MenuOpciones");

        TTScheck.Pressed += TTScheckbutton;
        //resume.GrabFocus();
        resume.Pressed += ResumeButton;
        CustomSignals.Instance.Connect(nameof(CustomSignals.Instance.Pausa), Callable.From(Pausa), (uint)GodotObject.ConnectFlags.Deferred);

    }

    public override void _Process(double delta)
    {

        if(Visible)
        {
            resume.Disabled = false;
            opciones.Disabled = false;
            salir.Disabled = false;
        }
        else
        {
            resume.Disabled = true;
            opciones.Disabled = true;
            salir.Disabled = true;
        }
        

    }

    public void Pausa()
    {
        if (!CustomSignals.pausado)
        {
            MenuOpciones.Visible = false;
            principal.Visible = true;
        }
    }
    private void _OnStartFocusEntered()
    {

        DisplayServer.TtsStop();

        string Message = resume.Text;
        if (CustomSignals.activado)
        {
            //CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }

    private void CambioOpciones()
    {
        SoundCues.GrabFocus();
       // musica.Stop();

    }
    private void CambioPrincipal()
    {
        resume.GrabFocus();
        //musica.Play();

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
        string Message = "volumen de Sound Cues";
        if (CustomSignals.activado)
        {
            //CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnMusicaSliderFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = "volumen de la musica";
        if (CustomSignals.activado)
        {
            //CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnTtsFocusEntered()
    {
        DisplayServer.TtsStop();
        string Message = "volumen de Text to Speech";
        if (CustomSignals.activado)
        {
          //  CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnTtsVelocidadFocusEntered()
    {
        DisplayServer.TtsStop();
        string Message = "Velocidad de Text to Speech";
        if (CustomSignals.activado)
        {
           // CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnRetunrButtonFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = retu.Text;
        if (CustomSignals.activado)
        {
           // CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnOpcionesFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = opciones.Text;
        if (CustomSignals.activado)
        {
           // CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnSalirFocusEntered()
    {
        DisplayServer.TtsStop();

        string Message = salir.Text;
        if (CustomSignals.activado)
        {
         //   CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    private void _OnCheckBoxEntered()
    {
        DisplayServer.TtsStop();
        string Message = "Quitar text to speech";
        if (CustomSignals.activado)
        {//CustomSignals.Instance.repetir = Message;

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    public void SwapMenu(int menuIndex, int returnIndex)
    {
        if (GetChild(menuIndex) is MenuTabPausa menutab)
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
        GD.Print("salir");
        GetTree().Quit();
    }

    private void ResumeButton()
    {
        CustomSignals.pausado = !CustomSignals.pausado;

        CustomSignals.Instance.EmitSignal("Pausa");
    }

    public void aparece()
    {
        resume.GrabFocus();
    }
}
