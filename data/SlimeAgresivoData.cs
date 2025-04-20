using Godot;
using System;

[GlobalClass]
public partial class SlimeAgresivoData : EnemyEntity
{
	public SlimeAgresivoData(int level){
		Level = level;
		
		this.estadoManager = new EstadoManager(this);
		
		Name = "Slime Enfadado";
		
		Mana = 0;
		
		ControlPlayer = false;
		Turn = false;
		
		beh_type = Behaviour.Agresivo;
		
		TrueHealth = new int[] {14,17,20,23,26,29,32,35,38,41};
		TrueAttack = new int[] {6,7,8,9,10,11,12,13,14,15};
		TrueDefense = new int[] {3,4,5,6,7,8,9,10,11,12};
		TrueMana = new int[] {0,0,0,0,0,0,0,0,0,0};
		TrueSpeed = new int[] {4,5,6,7,8,9,10,11,12,13};
		Health = TrueHealth[level-1];
		
		atqBasico = new SlimeSubditoMovimientoBasico(2);
	}
	
}
