using Godot;
using Godot.Collections;
using System;
using System.Threading; 

public partial class CinematicaInicial : Node2D
{
	// Called when the node enters the scene tree for the first time.

	private Sprite2D alex;
	private Sprite2D cassandra;
	private Sprite2D vyls;
	private Sprite2D ishi;
	private AudioStreamPlayer2D ruido; 
	
	public override void _Ready()
	{
		ruido = GetNode<AudioStreamPlayer2D>("ruido_puerta"); 
		alex = GetNode<Sprite2D>("Chuvakan");
		alex.Visible = false; 
		cassandra = GetNode<Sprite2D>("Watona");
		cassandra.Visible = false; 
		vyls = GetNode<Sprite2D>("Halo");
		vyls.Visible = false; 
		ishi = GetNode<Sprite2D>("Doraemon");
		ishi.Visible = false;
		Escena(); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //Si pulsas 'Enter', saltas la escena.
        if (Input.IsKeyLabelPressed(Key.Enter)) {
            GD.Print("Saltar escena"); 
        }
	}

	//Funcion que reproduce la escena 
	public async void Escena() {
		String mensaje = "Un grupo de cuatro heroes se encuentra en la entrada de un coliseo";

        if (CustomSignals.activado)
        {
            CustomSignals.Instance.repetir = mensaje;
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);

        }
        while (DisplayServer.TtsIsSpeaking())
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // Espera 100ms
        }

        //Introduccion de Alex
        alex.Visible = true;
		mensaje = "El primero de ellos es Alex, un joven con gorra y sudadera roja.";

        if (CustomSignals.activado)
        {
            CustomSignals.Instance.repetir = mensaje;
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);

        }
        while (DisplayServer.TtsIsSpeaking())
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // Espera 100ms
        }

        //Introduccion de Cassandra
        cassandra.Visible = true;
        mensaje = "Le sigue Cassandra, una maga de pelo negro y tunica del mismo color.";

        if (CustomSignals.activado)
        {
            CustomSignals.Instance.repetir = mensaje;
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);

        }
        while (DisplayServer.TtsIsSpeaking())
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // Espera 100ms
        }


        //Introduccion de Vyls
        vyls.Visible = true;
        mensaje = "Tambien se encuentra bils, un robot.....";

        if (CustomSignals.activado)
        {
            CustomSignals.Instance.repetir = mensaje;
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);

        }
        while (DisplayServer.TtsIsSpeaking())
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // Espera 100ms
        }


        //Introduccion de Ishi
        ishi.Visible = true;
        mensaje = "El ultimo miembro se trata de Ishimondo, una criatura humanoide azul con forma de gato, y una mascara que oculta su rostro.";

        if (CustomSignals.activado)
        {
            CustomSignals.Instance.repetir = mensaje;
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);

        }
        while (DisplayServer.TtsIsSpeaking())
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // Espera 100ms
        }

        //Entrada a batalla 
        mensaje = "Todos escuchan un ruido delante suya";
        if (CustomSignals.activado)
        {
            CustomSignals.Instance.repetir = mensaje;
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);

        }
        while (DisplayServer.TtsIsSpeaking())
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // Espera 100ms
        }

        ruido.Play(); 
    }


	private void On_ruido_puerta_finished() {
		GD.Print("Entrando a la batalla");
        CustomSignals.Instance.EmitSignal(nameof(CustomSignals.cinematica));
        alex.Visible = false;
        vyls.Visible = false;
        ishi.Visible = false;
        cassandra.Visible = false;

    }


}
