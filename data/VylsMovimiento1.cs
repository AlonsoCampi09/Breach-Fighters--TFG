using Godot;
using System;

public partial class VylsMovimiento1 : Movimiento{
	
	
	public VylsMovimiento1(int l){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
		
		this.evolucion = 6;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		this.putEffectsOnTargets(100, Estado.Regeneracion, 1, 20);
		if(this.casterLevel >= this.evolucion){
			this.putEffectsOnTargets(100, Estado.Energetico, 1, 20);
		}
		GD.Print("Vyls usa Ciclo de carga!");
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Ciclo de regeneración";
		}else{
			return "Ciclo de carga";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		if(this.casterLevel >= this.evolucion){
			coste = 4;
		}else{
			coste = 7;
		}
	}
}
