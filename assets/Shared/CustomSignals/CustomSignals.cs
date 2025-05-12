using Godot;
using System;

public partial class CustomSignals : Node
{

    [Signal]
    public delegate void PassTurn1EventHandler();
    [Signal]
<<<<<<< Updated upstream
    public delegate void PassTurn2EventHandler();
    [Signal]
    public delegate void PassTurn3EventHandler();
    [Signal]
    public delegate void PassTurn4EventHandler();
    [Signal]
    public delegate void RememberAEventHandler();
    [Signal]
    public delegate void RememberEEventHandler();
    [Signal]
    public delegate void OnDialogConfirmedEventHandler();
    [Signal]
    public delegate void OnMoveResolvedEventHandler();
    [Signal]
    public delegate void OnDialogRequestedEventHandler(string text);

    [Signal]
    public delegate void BattlefinishedEventHandler();
    public string[] voices = DisplayServer.TtsGetVoicesForLanguage("es");
    public string voiceId;
    public string repetir;

    public static CustomSignals Instance { get; private set; }
    public static bool activado { get; set; }
    public static int volumenTextToSpeach { get; set; }
    public static int velocidadTextToSpeach { get; set; }

    public override void _Ready()
    {
        activado = true;
        voiceId = voices[0];
        Instance = this;
        GD.Print("ready CustomSignals");
        volumenTextToSpeach = 50;
        velocidadTextToSpeach = 1;
=======
	public delegate void BattlefinishedEventHandler();
    [Signal]
    public delegate void PausaEventHandler();
    public string[] voices = DisplayServer.TtsGetVoicesForLanguage("es");
	public string voiceId;
	public string repetir;

	public static CustomSignals Instance { get; private set; }
	public static bool activado { get; set; }
    public static bool pausado { get; set; }

    public static bool cinematicaPausa { get; set; }


    public static int volumenTextToSpeach { get; set; }
	public static int velocidadTextToSpeach { get; set; }

	public override void _Ready()
	{
        pausado = false;
        cinematicaPausa = true;
        activado = true;
		voiceId = voices[0];
		Instance = this;
		GD.Print("ready CustomSignals");
		volumenTextToSpeach = 50;
		velocidadTextToSpeach = 1;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("RememberA")) // "RememberA" es 1
		{
			EmitSignal("RememberA"); // Emit the signal
		}
		if (Input.IsActionJustPressed("RememberE")) // "RememberA" es 2
		{
			EmitSignal("RememberE"); // Emit the signal
		}
		if (Input.IsActionJustPressed("repetir")) // "RememberA" es 2
		{
			repetirmensaje(); // Emit the signal
		}
        if (Input.IsActionJustPressed("pausa") && !cinematicaPausa)
        {
			pausado = !pausado;

            EmitSignal("Pausa");
		}
>>>>>>> Stashed changes
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("RememberA")) // "RememberA" es 1
        {
            EmitSignal("RememberA"); // Emit the signal
        }
        if (Input.IsActionJustPressed("RememberE")) // "RememberA" es 2
        {
            EmitSignal("RememberE"); // Emit the signal
        }
        if (Input.IsActionJustPressed("repetir")) // "RememberA" es 2
        {
            repetirmensaje(); // Emit the signal
        }
    }

    public void repetirmensaje()
    {
        DisplayServer.TtsStop();
        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(repetir, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
}
