using Godot;
using System;

public partial class VylsMovimiento2 : Movimiento{
	
	
	public VylsMovimiento2(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 4;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		GD.Print("Vyls usa Fragmentacion!");
		this.hurtTargets(potencia);
		if(this.casterLevel >= this.evolucion){
			this.putEffectsOnTargets(30, Estado.Aturdido, 1, 0);
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Fragmentación";
		}else{
			return "Tempestad";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Vyls lanza una granada que al explotar hace daño a todos los enemigos.";
		}else{
			return "Vyls lanza una granada que al explotar hace daño a todos los enemigos. Tiene un 30 porciento de probabilidad que Bloque las defensas de los afectados.";
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
		potencia = 5;
		if(this.casterLevel >= evolucion){
			coste = 7;
		}else{
			coste = 5;
		}
	}
	
}
