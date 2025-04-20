using Godot;
using System;

[GlobalClass]
public partial class SlimeVagoData : EnemyEntity
{
	public SlimeVagoData(int level){
		Level = level;
		
		this.estadoManager = new EstadoManager(this);
		
		Name = "Slime Vago";
		
		Mana = 0;
		
		ControlPlayer = false;
		Turn = false;
		
		beh_type = Behaviour.Aleatorio;
		
		TrueHealth = new int[] {15,18,21,24,27,30,33,36,39,42};
		TrueAttack = new int[] {4,5,6,7,8,9,10,11,12,13};
		TrueDefense = new int[] {6,7,8,9,10,11,12,13,14,15};
		TrueMana = new int[] {0,0,0,0,0,0,0,0,0,0};
		TrueSpeed = new int[] {2,3,4,5,6,7,8,9,10,11};
		
		Health = TrueHealth[level-1];
		
		atqBasico = new SlimeSubditoMovimientoBasico(1);
	}
	
}
