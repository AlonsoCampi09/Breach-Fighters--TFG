using Godot;
using System;

public partial class CassandraMovimiento1 : Movimiento{
	
	
	public CassandraMovimiento1(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 2;
		this.evolucion = 4;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		GD.Print("Cassandra va ha hacer su ataque especial!");
        this.hurtTargets(potencia);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < evolucion){
			return "Brecha umbría";
		}else{
			return "Ruptura";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < evolucion){
			return "Ataque especial que ataca a dos enemigos haciendo daño moderado.";
		}else{
			return "Movimiento especial que ataca a todos enemigos haciendo daño moderado.";
		}
	}
	public override bool affectsAllTeam(){
		if(this.casterLevel < evolucion){
			return false;
		}else{
			return true;
		}
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override void assingLevel(int l){
		this.casterLevel = l;
		if(this.casterLevel >= evolucion){
			potencia = 6;
			coste = 7;
		}else{
			potencia = 5;
			coste = 4;
		}
	}
}
