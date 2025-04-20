using Godot;
using System;

public partial class VylsMovimiento3 : Movimiento{
	
	
	public VylsMovimiento3(){
		this.effectObj = Effect_Obj.Both;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia, coste;
		potencia = 9 + this.casterLevel;
		if(this.casterLevel < 4)	
			coste = 4;
		else
			coste = 6;
		this.origen.passData().removeMP(coste);
		GD.Print("Vyls usa Reinicio!");
		if(!this.objetivos[0].passData().ControlPlayer)
			//this.hurtTargets(potencia);
		if(this.casterLevel < 6){
			if(!this.objetivos[0].passData().ControlPlayer){
				this.objetivos[0].passData().estadoManager.Reinicio_efecto();
			}else{
				this.objetivos[0].passData().estadoManager.Restauracion_efecto();
			}
		}else
			this.objetivos[0].passData().estadoManager.Reinicio_efecto();
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 6){
			return "Reinicio";
		}else{
			return "Restauración";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 4){
			return "Vyls limpia al objetivo de todos los estados que tenga (excepto el estado de envenendado). Si el objetivo es un aliado, este ataque no le hará daño.";
		}else{
			return "Vyls limpia al objetivo de todos los estados que tenga (excepto el estado de envenendado). Si el objetivo es un aliado, este ataque no le hará daño y le mantendrá los estados positivos.";
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
		if(this.casterLevel < 7){
			if(this.origen.passData().giveMP() >= 4){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 6){
				return true;
			}else
				return false;
		}
	}
	
}
