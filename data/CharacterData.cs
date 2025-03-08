using Godot;
using System;

[GlobalClass]
public partial class CharacterData : Resource
{
	[Export] public int Level { get; set; } = 1;
	[Export] public int Health { get; set; }= 1;
	[Export] public int Defense { get; set; }= 1;
	[Export] public int Attack { get; set; }= 1;
	[Export] public int Mana { get; set; }= 1;
	[Export] public int Speed { get; set; }= 1;
	
	[Export] public bool Turn { get; set; } = false;
	
	[Export] public int TrueHealth { get; set; } = 1;
	[Export] public int TrueDefense { get; set; }= 1;
	[Export] public int TrueAttack { get; set; }= 1;
	[Export] public int TrueMana { get; set; }= 1;
	[Export] public int TrueSpeed { get; set; }= 1;
	
	public virtual void LevelUp(){
		Level++;
	}
	
	public void isMyTurn(){
		Turn = true;
	}
	
	public void isNotLongerMyTurn(){
		Turn = false;
	}
}
