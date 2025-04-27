using Godot;
using System;

public partial class ChuvakanMovimiento2 : Movimiento{
	
	
	public ChuvakanMovimiento2(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 3;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		GD.Print("Alex va ha hacer su ataque especial!");
		this.hurtTargets(potencia);
		this.putEffectsOnTargets(100, Estado.DeBuffDEF, 2, 10);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Lanzamiento de gorra";
		}else{
			return "Lanzamiento de cuaderno";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		potencia = 4;
		if(this.casterLevel >= this.evolucion){
			coste = 7;
		}else{
			coste = 5;
		}
	}
	
}
