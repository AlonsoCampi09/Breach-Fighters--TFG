using Godot;
using System;

public partial class VylsMovimientoDefensivo : Movimiento{
	
	
	public VylsMovimientoDefensivo(){
		this.effectObj = Effect_Obj.Self;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int recuperacion_MP, def_ptg;
		if(this.casterLevel < 5){
			recuperacion_MP = (int) this.origen.passData().giveMAXMP()/4;
			def_ptg = 20;
		}else{
			recuperacion_MP = (int) this.origen.passData().giveMAXMP()/3;
			def_ptg = 30;
		}
		this.origen.passData().restoreMP(recuperacion_MP);
		this.origen.passData().estadoManager.AplicarEstado(Estado.BuffDEF,2,def_ptg);
		this.origen.ActualizarIconosEstado();
		GD.Print("Vyls va ha defenderse este turno!");
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 5){
			return "Cuártel";
		}else{
			return "Holograma tangible";
		}
	}
	public override string giveDescripcion(){
		return "Escudo que aumenta la defensa hasta el proximo turno. Tambien devuelve parte del maná";
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		return true;
	}
	
}
