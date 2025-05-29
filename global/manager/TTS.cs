using Godot;
using System;
using System.Collections.Generic;

public partial class TTS : Node{
	
	private static string[] voices;
	private static string voiceId;
	private static bool ttsEnabled;
	
	private static Queue<string> messageQueue = new Queue<string>();
	private static string lastMessage = "";
	private static bool isSpeaking = false;
	
	private static float volumenTTS = 1.0f; // de 0.0 a 1.0
	private static float velocidadTTS = 1.0f; // 1.0 normal, 0.5 lento, 2.0 rápido
	
	public override void _Ready(){
		voices = DisplayServer.TtsGetVoicesForLanguage("es");
		if (voices.Length > 0){
			voiceId = voices[0];
			ttsEnabled = true;
			SetProcess(true);
		}
		else{
			GD.PrintErr("No hay voces TTS disponibles.");
			ttsEnabled = false;
		}
	}
	
	public override void _Process(double delta){
		if (ttsEnabled && !isSpeaking && messageQueue.Count > 0){
			string siguiente = messageQueue.Dequeue();
			lastMessage = siguiente;
			isSpeaking = true;
			DisplayServer.TtsSpeak(siguiente, voiceId, (int)volumenTTS * 2, 1, velocidadTTS);
			// Simulamos que está hablando — en Godot no hay aún callback en C#
			// Usamos un temporizador como aproximación si quieres
			// Aquí asumimos que un nuevo mensaje no se dispara mientras se habla
			GetTree().CreateTimer(0.5f).Timeout += () => { isSpeaking = false; };
		}
	}
	
	public static void SayThis(string msg){
		if (!ttsEnabled)
			return;
		StopTTS();
		messageQueue.Enqueue(msg);
	}
	public static void PutThisInQueue(string msg){
		if (!ttsEnabled)
			return;
		messageQueue.Enqueue(msg);
	}
	public static void StopTTS(){
		DisplayServer.TtsStop();
		messageQueue.Clear();
		isSpeaking = false;
	}
	
	public static void RepeatLast(){
		if (!string.IsNullOrEmpty(lastMessage)){
			DisplayServer.TtsStop(); // Por si acaso está hablando
			messageQueue.Enqueue(lastMessage);
			isSpeaking = false; // Forzamos a reproducir en el próximo _Process()
		}
	}
	
	public static void EnableDisableTTS(bool e){
		ttsEnabled = e;
		if (!ttsEnabled){
			DisplayServer.TtsStop();
			messageQueue.Clear();
			isSpeaking = false;
			SayThis("Texto a voz desactivado.");
		}else{
			messageQueue.Clear();
			SayThis("Texto a voz activado. Encantada de servirte.");
		}
	}
	
	public static void SetSpeed(float speed){
		velocidadTTS = Mathf.Clamp(speed, 0.5f, 3.0f);
	}
	public static void SetVolume(float volume){
		volumenTTS = Mathf.Clamp(volume, 10.0f, 50.0f);
	}
}
