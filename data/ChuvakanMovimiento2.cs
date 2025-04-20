using Godot;
using System;

public partial class ChuvakanMovimiento2 : Movimiento{
	
	
	public ChuvakanMovimiento2(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia = 4, coste;
		if(this.casterLevel < 5){
			coste = 4;
		}else{
			coste = 7;
		}
		this.origen.passData().removeMP(coste);
		GD.Print("Alex va ha hacer su ataque especial!");
		//this.hurtTargets(potencia);
		for(int i = 0; i < objetivos.Count; i++){
			this.objetivos[i].passData().estadoManager.AplicarEstado(Estado.DeBuffDEF,2,10);
			this.objetivos[i].ActualizarIconosEstado();
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 3){
			return "Lanzamiento de gorra";
		}else{
			return "Lanzamiento de cuaderno";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 3){
			return "Alex lanza su gorra a un enemigo lo que hace que la defensa del enemigo en un 10 porciento durante sus proximos 2 turnos ademas de hacerle un poco de daño.";
		}else{
			return "Alex lanza su cuaderno a los enemifos lo que hace que la defensa de todos los afectados en un 10 porciento durante sus proximos 2 ademas de hacerle un poco de daño.";
		}
	}
	public override bool affectsAllTeam(){
		if(this.casterLevel < 5){
			return false;
		}else{
			return true;
		}
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		if(this.casterLevel < 5){
			if(this.origen.passData().giveMP() >= 5){
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
