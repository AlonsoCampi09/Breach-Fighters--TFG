using Godot;
using System;

public partial class CassandraMovimiento4 : Movimiento{
	
	
	public CassandraMovimiento4(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia, coste, probabilidad = 30, random_number;
		Random rand = new Random();
		random_number = rand.Next(1, 101);
		if(this.casterLevel < 7){
			potencia = 10 + this.casterLevel;
			coste = 10;
		}else{
			potencia = 10 + (int) (1.5 * this.casterLevel);
			coste = 12;
		}
		this.origen.passData().removeMP(coste);
		
		GD.Print("Cassandra va ha hacer su ataque especial!");
		//this.hurtTargets(potencia);
		if(random_number >= 100-probabilidad){
			this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.DeBuffDMG,1,25);
			this.objetivos[0].ActualizarIconosEstado();
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 7){
			return "Juicio";
		}else{
			return "Exilio";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 7){
			return "Cassandra transmite gran parte de su magia en este conjuro, haciendo gran daño al objetivo y pudiendo causar una reduccion de daño del 25% en su siguiente turno.";
		}else{
			return "Cassandra transmite gran parte de su magia en este conjuro, haciendo gran daño al objetivo y causandole una reduccion de daño del 25% en su siguiente turno.";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		if(this.casterLevel < 3){
			return false;
		}else{
			return true;
		}
	}
	public override bool enoughMana(){
		if(this.casterLevel < 7){
			if(this.origen.passData().giveMP() >= 10){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 12){
				return true;
			}else
				return false;
		}
	}
	
}
