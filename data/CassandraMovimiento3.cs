using Godot;
using System;

public partial class CassandraMovimiento3 : Movimiento{
	
	
	public CassandraMovimiento3(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia, coste, curacion;
		if(this.casterLevel < 6){
			potencia = 5 + this.casterLevel;
			coste = 4;
		}else{
			potencia = 5 + (int) (1.5 * this.casterLevel);
			coste = 7;
		}
		this.origen.passData().removeMP(coste);
		this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.Sellado,1,0);
		this.objetivos[0].ActualizarIconosEstado();
		GD.Print("Cassandra va ha hacer su ataque especial!");
		//curacion = (int) (this.hurtTargets(potencia) / 2);
		if(this.casterLevel <= 6){
			//this.origen.passData().restoreHP(curacion);
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 6){
			return "Sello arcano";
		}else{
			return "Marca rúnica";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 6){
			return "Cassandra aplica un hechizo neutralizador a un objetivo haciendole un poco de daño y aplicandole el efecto de Sellado durante el siguiente turno del objetivo.";
		}else{
			return "Cassandra aplica un hechizo drenador a un objetivo haciendole daño y aplicandole el efecto de Sellado durante el siguiente turno del objetivo. Además se cura 50% del daño realizado.";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		if(this.casterLevel < 2){
			return false;
		}else{
			return true;
		}
	}
	public override bool enoughMana(){
		if(this.casterLevel < 6){
			if(this.origen.passData().giveMP() >= 5){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 10){
				return true;
			}else
				return false;
		}
	}
	
}
