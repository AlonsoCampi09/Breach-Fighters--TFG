using Godot;
using System;

public partial class CassandraMovimientoBasico : Movimiento{
	
	
	public CassandraMovimientoBasico(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia;
		potencia = 10 + this.casterLevel;
		if(this.casterLevel >= 5){
			this.origen.passData().restoreMP((int) this.origen.passData().giveMAXMP()/6);
		}
		GD.Print("Cassandra va ha hacer su ataque basico!");
		//this.hurtTargets(potencia);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 5){
			return "Pulso magico";
		}else{
			return "Impulso arcano";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 5){
			return "Un disparo magico que hace daño a un enemigo.";
		}else{
			return "Un disparo magico que hace daño a un enemigo y restaura un podo de maná";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		return true;
	}
	
}
