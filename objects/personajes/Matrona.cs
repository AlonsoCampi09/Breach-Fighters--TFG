using Godot;
using System;

public partial class Matrona : CharacterBody2D
{
	private CharacterData data;
	private AnimatedSprite2D animSprite;
	
	public void ready(){
		data = new MatronaData();
		animSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	public override void _Process(double delta)
	{
		if(data.Health > (int) data.TrueHealth / 4){
			animSprite.Play("idle");
		}
		else{
			if(data.Health == 0){
				animSprite.Play("fainted");
			}else{
				animSprite.Play("idle_low");
			}
		}
	}
}
