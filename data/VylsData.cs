using Godot;
using System;

[GlobalClass]
public partial class VylsData : Entity
{
	
	public VylsData(int level){
		Level = level;
        this.ID = ID_generator;
        ID_generator++;
        Name = "bils";
		this.estadoManager = new EstadoManager(this);
		
		ControlPlayer = true;
		Turn = false;
		
		TrueHealth = new int[] {24,26,28,30,32,34,36,38,40,42};
		TrueAttack = new int[] {4,6,8,10,12,14,16,18,20,22};
		TrueDefense = new int[] {7,9,11,13,15,17,19,21,23,25};
		TrueMana = new int[] {15,17,19,21,23,25,27,29,31,33};
		TrueSpeed = new int[] {3,4,5,6,7,8,9,10,11,12};
		
		Health = TrueHealth[level-1];
		Mana = TrueMana[level-1];
		
		mov1 = new VylsMovimiento1();
		mov2 = new VylsMovimiento2();
		mov3 = new VylsMovimiento3();
		mov4 = new VylsMovimiento4();
		atqBasico = new VylsMovimientoBasico();
		defBasico = new VylsMovimientoDefensivo();
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
