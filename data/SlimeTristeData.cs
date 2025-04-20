using Godot;
using System;

[GlobalClass]
public partial class SlimeTristeData : EnemyEntity
{
	public SlimeTristeData(int level){
		Level = level;
		
		this.estadoManager = new EstadoManager(this);
		
		Name = "Slime Triste";
		
		Mana = 0;
		
		ControlPlayer = false;
		Turn = false;
		
		beh_type = Behaviour.Aleatorio;
		
		TrueHealth = new int[] {14,17,18,21,24,27,30,33,36,39};
		TrueAttack = new int[] {3,4,5,6,7,8,9,10,11,12};
		TrueDefense = new int[] {2,3,4,5,6,7,8,9,10,11};
		TrueMana = new int[] {0,0,0,0,0,0,0,0,0,0};
		TrueSpeed = new int[] {5,6,7,8,9,10,11,12,13,14};
		Health = TrueHealth[level-1];
		
		atqBasico = new SlimeSubditoMovimientoBasico(3);
	}
	
}
