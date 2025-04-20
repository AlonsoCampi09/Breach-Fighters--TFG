using Godot;
using System;

public partial class VylsMovimiento4 : Movimiento{
	
	
	public VylsMovimiento4(){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int coste;
		if(this.casterLevel < 9)	
			coste = 7;
		else
			coste = 15;
		this.origen.passData().removeMP(coste);
		GD.Print("Vyls usa vanguardia!");
		if(this.casterLevel < 9)
			this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.BuffDEF,1,50);
		else
			this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.Vanguardia,1,0);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 6){
			return "Protocolo: COBERTURA";
		}else{
			return "Protocolo: VANGUARDIA";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 4){
			return "Vyls alza sus defensas para proteger a un miembro del equipo lo que aumenta su defensa en un 50 porciento.";
		}else{
			return "Vyls alza sus defensas para proteger a un miembro del equipo lo que le protege de todo daño hasta su proximo turno.";
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
		if(this.casterLevel < 9){
			if(this.origen.passData().giveMP() >= 7){
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
