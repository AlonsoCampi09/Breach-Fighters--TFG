using Godot;
using System;

public partial class CassandraMovimientoBasico : Movimiento{
	
	
	public CassandraMovimientoBasico(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		evolucion = 5;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		if(this.casterLevel >= evolucion){
			this.origen.passData().restoreMP((int) this.origen.passData().giveMAXMP()/6);
		}
		GD.Print("Cassandra va ha hacer su ataque basico!");
		this.hurtTargets(potencia);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < evolucion){
			return "Pulso magico";
		}else{
			return "Impulso arcano";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		coste = 0;
		potencia = 8;
	}

	
}
