using Godot;
using System;

public partial class VylsMovimiento1 : Movimiento{
	
	
	public VylsMovimiento1(){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int coste;
		if(this.casterLevel < 6){	
			coste = 4;
		}else{
			coste = 7;
		}
		this.origen.passData().removeMP(coste);
		for(int i = 0; i < objetivos.Count; i++){
			this.objetivos[i].passData().estadoManager.AplicarEstado(Estado.Regeneracion,3,0);
			if(this.casterLevel >= 6){	
				this.objetivos[i].passData().estadoManager.AplicarEstado(Estado.Energetico,3,0);
			}
			this.objetivos[i].ActualizarIconosEstado();
		}
		GD.Print("Vyls usa Ciclo de carga!");
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 6){
			return "Ciclo de regeneración";
		}else{
			return "Ciclo de carga";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 6){
			return "Vyls prepara el campo de batalla para sus aliados, dandoles el estado de Regeneración durante los proximos 3 turnos.";
		}else{
			return "Vyls prepara el campo de batalla para sus aliados, dandoles el estado de Regeneración y Enérgico durante los proximos 3 turnos.";
		}
	}
	public override bool affectsAllTeam(){
		return true;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		if(this.casterLevel < 6){
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
