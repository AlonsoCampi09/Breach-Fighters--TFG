using Godot;
using System;
using System.Collections.Generic;

public partial class EstadoDisplay : Node2D
{
	[Export] public float tiempoCambio = 1.5f; // Tiempo entre cambio de sprite
	private Sprite2D sprite;
	private Timer timer;

	private List<Texture2D> iconos = new List<Texture2D>();
	private int indiceActual = 0;

	public override void _Ready(){
		sprite = GetNode<Sprite2D>("Sprite2D");
		timer = new Timer();
		timer.WaitTime = tiempoCambio;
		timer.OneShot = false;
		timer.Timeout += CambiarIcono;
		AddChild(timer);
		timer.Start();
	}

	public void SetIconos(List<Texture2D> nuevosIconos){
		iconos = nuevosIconos;
		indiceActual = 0;
		MostrarIconoActual();
	}

	private void CambiarIcono(){
		if (iconos.Count == 0){
			sprite.Texture = null;
			return;
		}
		indiceActual = (indiceActual + 1) % iconos.Count;
		MostrarIconoActual();
	}
	private void MostrarIconoActual(){
		sprite.Texture = iconos[indiceActual];
	}
}
