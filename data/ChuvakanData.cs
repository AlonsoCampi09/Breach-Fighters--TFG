using Godot;
using System;

[GlobalClass]
public partial class ChuvakanData : CharacterData
{

	
	public ChuvakanData()
	{
		Level = 1;
		Health = 20;
		Defense = 5;
		Attack = 5;
		Mana = 15;
		Speed  = 5;
		TrueHealth = 20;
		TrueDefense = 5;
		TrueAttack = 5;
		TrueMana = 15;
		TrueSpeed  = 5;
	}
	
	public override void LevelUp()
	{
		Level++;
		Health += 3;
		Defense++; 
		Attack++;
		Mana += 2;   
		Speed += 2; 
		
		TrueHealth += 3;
		TrueDefense++; 
		TrueAttack++;
		TrueMana += 2;   
		TrueSpeed += 2; 
	}
	
}
