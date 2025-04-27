using Godot;
using System;

public partial class VylsMovimiento4 : Movimiento{
	
	
	public VylsMovimiento4(int l){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
		
		this.evolucion = 9;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		GD.Print("Vyls usa vanguardia!");
		if(this.casterLevel < this.evolucion)
			this.putEffectsOnTargets(100, Estado.BuffDEF, 1, 50);
		else
			this.putEffectsOnTargets(100, Estado.Vanguardia, 1, 0);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Protocolo: COBERTURA";
		}else{
			return "Protocolo: VANGUARDIA";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		if(this.casterLevel >= this.evolucion){
			coste = 15;
		}else{
			coste = 7;
		}
	}
}
