using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FighterTeam : Node2D
{	

	private List<Fighter> fighters = new List<Fighter>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("SOY READY TEAM");
		prepare_TeamList();
	}
	
	public void prepare_TeamList(){
	Random rand = new Random(); 
	foreach (Fighter e in GetChildren()){
		int level = e.passData().ControlPlayer==true ? 1 : rand.Next(1, 4);
		e.prepareFighter(level);
		fighters.Add(e);
		
		Entity entityData = e.passData();
		if (entityData != null){
			GD.Print("PassData FUNCIONA");
		}
	}
	GD.Print("Imprimiendo STATS");
	foreach (Fighter f in fighters){
		Entity c = f.passData();
		GD.Print($"NAME: {c.Name}");
		GD.Print($"LEVEL: {c.Level}");
		GD.Print($"HP: {c.giveMAXHP()}");
		GD.Print($"MP: {c.giveMAXMP()}");
		GD.Print($"DMG: {c.giveDMG()}");
		GD.Print($"DEF: {c.giveDEF()}");
		GD.Print($"SP: {c.giveSP()}");
		GD.Print("---------------------------");
	}
}
	
	public List<Fighter> giveList(){
		return fighters;
	}
}
