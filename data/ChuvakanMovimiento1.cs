using Godot;
using System;

public partial class ChuvakanMovimiento1 : Moviento{
	
	
	public ChuvakanMovimiento1(){
		this.effectObj = Effect_Obj.Ally;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		GD.Print("Chuvakan usa Motivación o Motivación inspirativa!");
	}
	
	public override string giveTitulo(int level){
		if(level < 3){
			return "Motivación";
		}else{
			return "Motivación inspirativa";
		}
	}
	public override string giveDescripcion(int level){
		if(level < 3){
			return "Movimiento especial que aumenta el ataque y la defensa en un 20% de si mismo o de un compañero activo del grupo en su próximo turno.";
		}else{
			return "Movimiento especial que aumenta el ataque y la defensa en un 20% de si mismo y de un compañero activo del grupo en sus próximos 2 turnos.";
		}
	}
	
}
