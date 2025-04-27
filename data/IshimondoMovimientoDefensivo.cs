using Godot;
using System;

public partial class IshimondoMovimientoDefensivo : Movimiento{
	
	
	public IshimondoMovimientoDefensivo(int l){
		this.effectObj = Effect_Obj.Self;
		this.num_objetivos = 1;
		this.evolucion = 5;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int recuperacion_MP, def_ptg;
		def_ptg = 10;
		recuperacion_MP = (int) this.origen.passData().giveMAXMP()/3;
		this.origen.passData().restoreMP(recuperacion_MP);
		this.putEffectsOnTargets(100, Estado.BuffDEF, 2, def_ptg);
		if(this.casterLevel >= this.evolucion)
			this.putEffectsOnTargets(100, Estado.Evasion, 2, 0);
		GD.Print("Ishimondo va ha defenderse este turno!");
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Hacerse bolita";
		}else{
			return "Reflejps felinos";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Maniobra que aumenta la defensa hasta el proximo turno. Tambien devuelve parte del maná.";
		}else{
			return "Maniobra que aumenta la defensa hasta el proximo turno. Tambien devuelve parte del maná y le da el estado evasivo.";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	
}
