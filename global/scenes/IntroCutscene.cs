using Godot;
using System;

public partial class IntroCutscene : CanvasLayer{
	
	private int etapa = 0;
	private IntroTexts texts = new IntroTexts();
	private AnimationPlayer anim;
	private AudioStreamPlayer2D mus;
	private AudioStreamPlayer2D puerta;
	
	private CustomSignals customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		anim = GetNode<AnimationPlayer>("Anim");
		mus = GetNode<AudioStreamPlayer2D>("Music");
		puerta = GetNode<AudioStreamPlayer2D>("Puerta");
		
		customSignals.OnDialogIsOver += ContinueCutscene;
		puerta.Finished += OnSoundFinished;
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), texts.TextChunks[etapa]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void ContinueCutscene(){
		etapa++;
		switch(etapa){
			case 1:
				anim.Play("intro0");
				TTS.SayThis("Aparece una imagen en la cual salen unos orbes con lo que parece ser un ojo rodeando a uno más grande. El texto continua diciendo,");
				break;
			case 6:
				anim.Play("intro1");
				break;
			case 8:
				anim.Play("intro2");
				TTS.SayThis("La imagen cambia a una en la que el orbe del centro adquiere un rostro con un sonrisa inquietante. El texto continua diciendo,");
				break;
			case 10:
				anim.Play("intro3");
				TTS.SayThis("Aparece otra imagen de una mano sosteniendo un orbe que refleja a dos figuras. El texto continua diciendo,");
				break;
			case 13:
				anim.Play("intro4");
				TTS.SayThis("La imagen cambia con la mano estrujando el orbe con el rostro de EXITIO de fondo. El texto continua diciendo,");
				break;
			case 14:
				anim.Play("intro5");
				break;
			case 15:
				anim.Play("intro6");
				TTS.SayThis("Aparecen las siluetas de 4 combatientes. El texto continua diciendo, ");
				break;
			case 17:
				anim.Play("intro7");
				TTS.SayThis("La primera silueta se revela, dejando ver a un monigote blanco vestido con una sudadera y una gorra roja echada para atrás dejando escapar su pelo hacia arriba por el agujero de la gorra. Sostiene flotando un papel con un dibujo de un martillo. El texto continua presentadole.");
				break;
			case 18:
				anim.Play("intro8");
				TTS.SayThis("La segunda silueta se revela, dejando ver a una chica seria y algo gótica de ojos marrones con un brillo amarillo en ellos. Tiene un pelo negro corto y va vestida con un traje negro con una capa enlazada con una joya morada. Parece estar canalizando magia con su mano. El texto continua presentadola.");
				break;
			case 19:
				anim.Play("intro9");
				TTS.SayThis("La tercera silueta se revela, dejando ver a alguien en un traje de combate futurista de color negro con un visor naranja. Tiene unos tubos conectados a su torso con energía azul dentro. Está apuntando un rifle de energía. El texto continua presentadole.");
				break;
			case 20:
				anim.Play("intro10");
				TTS.SayThis("La última silueta se revela, dejando ver a un gato antropomórfico de pelaje azul con una máscara que deja ver sus ojos negros con pupilas blancas. Está mostrando sus garras y parece determinado. El texto continua presentadole.");
				break;
			case 21:
				anim.Play("intro11");
				break;
			case 25:
				TTS.StopTTS();
				puerta.Play();
				return;
		}
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), texts.TextChunks[etapa]);
	}
	
	private void OnSoundFinished(){
		GD.Print("Sonido terminado. Cambiando de escena...");
		GetTree().ChangeSceneToFile("res://global/scenes/game.tscn");
	}
	public override void _ExitTree(){
		customSignals.OnDialogIsOver -= ContinueCutscene; // o ShowDialog si es el que usas
	}
}
