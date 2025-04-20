using Godot;
using System;

public partial class ChuvakanMovimiento1 : Movimiento{
	
	
	public ChuvakanMovimiento1(){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int coste;
		if(this.casterLevel < 3){	
			coste = 5;
		}else{
			coste = 7;
		}
		this.origen.passData().removeMP(coste);
		this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.BuffDMG,1,20);
		this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.BuffDEF,1,20);
		this.objetivos[0].ActualizarIconosEstado();
		if(this.casterLevel >= 3){	
			this.origen.passData().estadoManager.AplicarEstado(Estado.BuffDMG,2,20);
			this.origen.passData().estadoManager.AplicarEstado(Estado.BuffDEF,2,20);
			this.origen.ActualizarIconosEstado();
		}
		GD.Print("Chuvakan usa Motivación!");
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 3){
			return "Motivación";
		}else{
			return "Motivación inspiradora";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 3){
			return "Alex motiva a uno de sus compañeros lo que aumenta el ataque y la defensa en un 20%.";
		}else{
			return "Alex motiva a uno de sus compañeros con confianza lo que aumenta el ataque y la defensa en un 20% tanto al aliado como a él";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		if(this.casterLevel < 3){
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
