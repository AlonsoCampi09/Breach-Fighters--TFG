using Godot;
using System;

public partial class Flecha : Node2D
{
	private Sprite2D arrowIndicator;

	public override void _Ready()
	{
		arrowIndicator = GetNode<Sprite2D>("Arrow"); // Asegúrate de que la flecha está en la escena
		arrowIndicator.Visible = false; // Ocultar por defecto
	}

	public void ShowArrow(bool show)
	{
		arrowIndicator.Visible = show;
	}
	
	public void MoveArrow(Vector2 position)
	{
		Position = position; // Actualizar la posición de la flecha
	}
}
