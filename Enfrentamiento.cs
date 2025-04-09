using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Enfrentamiento : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private PackedScene[] equipos; // Array de equipos para elegir aleatoriamente
	private int equipoSelect = 0;
	private FighterTeam equipoInstanciado;

	public override void _Ready()
	{
		GD.Print("SOY READY ENFRENTAMIENTO");
		if (equipos.Length == 0)
		{
			GD.PrintErr("No hay equipos disponibles para el enfrentamiento.");
			return;
		}
		// Seleccionar un equipo aleatorio
		Random rand = new Random();
		int index = rand.Next(0, equipos.Length);
		equipoSelect = index;
		PackedScene equipoSeleccionado = equipos[equipoSelect];

		// Instanciar el equipo en la escena
		equipoInstanciado = equipoSeleccionado.Instantiate<FighterTeam>();
		AddChild(equipoInstanciado);

		GD.Print($"Equipo seleccionado: {equipoSeleccionado.ResourcePath}");
	}
	
	public List<Fighter> giveList(){
		return equipoInstanciado.giveList();
	}

	
}
