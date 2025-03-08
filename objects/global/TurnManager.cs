using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TurnManager : Node
{
	private List<CharacterData> turnQueue = new List<CharacterData>();
	private List<CharacterData> characters = new List<CharacterData>();
	
	public void doList(){
		// Ordenamos por velocidad
		turnQueue = characters.OrderByDescending(c => c.Speed).ToList(); 
	}
	
	public void giveMeCharacterLists(List<CharacterData> c){
		characters = c	;
	}
	
}
