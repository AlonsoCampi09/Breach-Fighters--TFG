using Godot;
using System;

public partial class VylsMovimiento2 : Movimiento{
	
	
	public VylsMovimiento2(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia, coste;
		if(this.casterLevel < 4){	
			potencia = 6 + this.casterLevel;
			coste = 4;
		}else{
			potencia = 8 + this.casterLevel;
			coste = 7;
		}
		this.origen.passData().removeMP(coste);
		GD.Print("Vyls usa Fragmentacion!");
		//this.hurtTargets(potencia);
		if(this.casterLevel >= 6){
			Random rand = new Random();
			int random_number = rand.Next(1, 4);
			for(int i = 0; i < objetivos.Count; i++){
				random_number = rand.Next(1, 4);
				if(random_number ==  3){
					this.objetivos[i].passData().estadoManager.AplicarEstado(Estado.Bloqueo,1,0);
					this.objetivos[i].ActualizarIconosEstado();
				}
			}
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 6){
			return "Fragmentación";
		}else{
			return "Tempestad";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 4){
			return "Vyls lanza una granada que al explotar hace daño a todos los enemigos.";
		}else{
			return "Vyls lanza una granada que al explotar hace daño a todos los enemigos. Tiene un 30 porciento de probabilidad que Bloque las defensas de los afectados.";
		}
	}
	public override bool affectsAllTeam(){
		return true;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		if(this.casterLevel < 4){
			if(this.origen.passData().giveMP() >= 5){
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
