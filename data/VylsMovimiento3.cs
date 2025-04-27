using Godot;
using System;

public partial class VylsMovimiento3 : Movimiento{
	
	
	public VylsMovimiento3(int l) {
		this.effectObj = Effect_Obj.Both;
		this.num_objetivos = 1;
		this.evolucion = 6;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		GD.Print("Vyls usa Reinicio!");
		if(!this.objetivos[0].passData().ControlPlayer)
			this.hurtTargets(potencia);
		if(this.casterLevel < 6){
			if(!this.objetivos[0].passData().ControlPlayer){
				this.objetivos[0].passData().estadoManager.Reinicio_efecto();
			}else{
				this.objetivos[0].passData().estadoManager.Restauracion_efecto();
			}
		}else
			this.objetivos[0].passData().estadoManager.Reinicio_efecto();
		this.objetivos[0].ActualizarIconosEstado();
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Reinicio";
		}else{
			return "Restauración";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		potencia = 7;
		if(this.casterLevel >= evolucion){
			coste = 7;
		}else{
			coste = 4;
		}
	}
	
}
