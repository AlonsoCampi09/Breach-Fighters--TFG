using Godot;
using System;

public partial class CassandraMovimiento1 : Movimiento{
	
	
	public CassandraMovimiento1(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 2;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia, coste;
		if(this.casterLevel > 4){
			potencia = 8 + this.casterLevel;
			coste = 4;
		}else{
			potencia = 7 + this.casterLevel;
			coste = 7;
		}
		this.origen.passData().removeMP(coste);
		GD.Print("Cassandra va ha hacer su ataque especial!");
		//this.hurtTargets(potencia);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 4){
			return "Brecha umbría";
		}else{
			return "Ruptura";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 4){
			return "Ataque especial que ataca a dos enemigos haciendo daño moderado.";
		}else{
			return "Movimiento especial que ataca a todos enemigos haciendo daño moderado.";
		}
	}
	public override bool affectsAllTeam(){
		if(this.casterLevel < 4){
			return false;
		}else{
			return true;
		}
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		if(this.casterLevel < 4){
			if(this.origen.passData().giveMP() >= 4){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 7){
				return true;
			}else
				return false;
		}
	}
	
}
