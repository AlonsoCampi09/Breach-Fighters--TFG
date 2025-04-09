using Godot;
using System;

public partial class CustomSignals : Node
{

    [Signal]
    public delegate void PassTurnEventHandler();
    [Signal]
    public delegate void PassTurn2EventHandler();
    [Signal]
    public delegate void PassTurn3EventHandler();
    [Signal]
    public delegate void PassTurn4EventHandler();
    public string[] voices = DisplayServer.TtsGetVoicesForLanguage("es");
    public string voiceId;
    public static CustomSignals Instance { get; private set; }

    public override void _Ready()
    {
        voiceId = voices[0];
        Instance = this;
        GD.Print("ready CustomSignals");
    }

}
