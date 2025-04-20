using Godot;
using System;

public partial class IshimondoMovimiento3 : Movimiento{
	
	
	public IshimondoMovimiento3(){
		this.effectObj = Effect_Obj.Self;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int coste;
		if(this.casterLevel < 5)
			coste = 6;
		else
			coste = 11;
		
		this.origen.passData().removeMP(coste);
		if(this.casterLevel >= 8){
			this.origen.passData().estadoManager.AplicarEstado(Estado.BuffDMG,3,50);
			this.origen.passData().estadoManager.AplicarEstado(Estado.Sellado,3,0);
			this.origen.passData().estadoManager.AplicarEstado(Estado.Bloqueo,3,0);
		}else{
			this.origen.passData().estadoManager.AplicarEstado(Estado.DeBuffDEF,3,15);
			this.origen.passData().estadoManager.AplicarEstado(Estado.BuffDMG,3,30);
		}
		this.origen.ActualizarIconosEstado();
		GD.Print("Ishimondo usa Despertar del instinto!");
	}
	public override string giveTitulo(){
		if(this.casterLevel < 8){
			return "Despertar del instinto";
		}else{
			return "Despertar de la instinto";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 8){
			return "Ishimondo pierde el control de sus acciones lo que le impulsa a solo atacar. Ishi gana un bufo del ataque 50 porciento y sufre sellado y bloqueo de guardia durante 3 turnos";
		}else{
			return "Ishimondo despierta su parte más salvaje. Durante los siguientes 3 turnos Ishi sufre una reduccion de su defensa del 15 porciento y un aumento de ataque del 30 porciento.";
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
		if(this.casterLevel < 8){
			if(this.origen.passData().giveMP() >= 6){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 15){
				return true;
			}else
				return false;
		}
	}
	
}
