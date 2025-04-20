using Godot;
using System;

[GlobalClass]
public partial class IshimondoData : Entity
{
	
	public IshimondoData(int level){
		Level = level;
		this.estadoManager = new EstadoManager(this);
		
		Name = "Ishimondo";
		
		ControlPlayer = true;
		Turn = false;
		
		TrueHealth = new int[] {16,18,20,22,24,26,28,30,32,34};
		TrueAttack = new int[] {8,10,12,14,16,18,20,22,24,26};
		TrueDefense = new int[] {4,5,6,7,8,9,10,11,12,13};
		TrueMana = new int[] {15,17,19,21,23,25,27,29,31,33};
		TrueSpeed = new int[] {7,9,11,13,15,17,19,21,23,25};
		
		Health = TrueHealth[level-1];
		Mana = TrueMana[level-1];
		
		mov1 = new IshimondoMovimiento1();
		mov2 = new IshimondoMovimiento2();
		mov3 = new IshimondoMovimiento3();
		mov4 = new IshimondoMovimiento4();
		atqBasico = new IshimondoMovimientoBasico();
		defBasico = new IshimondoMovimientoDefensivo();
		
	}
	
	public override void levelUp(){
		Level++;
		Health += 2;
		Mana += 2;
		this.atqBasico.assingLevel(Level);
		this.mov1.assingLevel(Level);
		this.mov2.assingLevel(Level);
		this.mov3.assingLevel(Level);
		this.mov4.assingLevel(Level);
		this.defBasico.assingLevel(Level);
	}
}
