using Godot;
using System;

[GlobalClass]
public partial class MatronaData : CharacterData
{
	public MatronaData()
	{
		Level = 1;
		Health = 18;
		Defense = 4;
		Attack = 7;
		Mana = 18;
		Speed  = 6;
		TrueHealth = 18;
		TrueDefense = 4;
		TrueAttack = 7;
		TrueMana = 18;
		TrueSpeed  = 6;
	}
	
	public override void LevelUp()
	{
		Level++;
		Health += 2;
		Defense++; 
		Attack += 2;
		Mana += 3;   
		Speed++; 
		
		TrueHealth += 2;
		TrueDefense++; 
		TrueAttack+= 2;
		TrueMana += 3;   
		TrueSpeed++; 
	}
}
