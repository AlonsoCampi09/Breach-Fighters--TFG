using Godot;
using System;

public partial class SlimeSubditoMovimientoBasico : Movimiento{
	
	private int slime_type;
	public SlimeSubditoMovimientoBasico(int slime){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		slime_type = slime;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia, random_number;
		if(this.casterLevel < 5){
			switch(slime_type){
				case 0:
					potencia = 6;
					break;
				case 1:
					potencia = 6;
					break;
				case 2:
					potencia = 7;
					break;
				case 3:
					potencia = 5;
					break;
			}
		}else{
			switch(slime_type){
				case 0:
					potencia = 7;
					break;
				case 1:
					potencia = 7;
					break;
				case 2:
					potencia = 8;
					break;
				case 3:
					potencia = 6;
					break;
			}
		}
		GD.Print("Slime va ha hacer su ataque basico!");
		//this.hurtTargets(potencia);
		Random rand = new Random();
		random_number = rand.Next(0, 11);
		if(random_number == 10){
			switch(slime_type){
				case 0:
					this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.DeBuffVEL,1,20);
					break;
				case 1:
					this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.DeBuffDMG,1,10);
					break;
				case 2:
					this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.DeBuffDEF,1,10);
					break;
				case 3:
					this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.Sellado,1,0);
					break;
			}
			this.objetivos[0].ActualizarIconosEstado();
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 5){
			return "Babeo";
		}else{
			switch(slime_type){
				case 0:
					return "Latigazo";
				case 1:
					return "Desliz";
				case 2:
					return "Quiebro";
				case 3:
					return "Lagrimas";
			}
		}
		return "";
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 5){
			return "Un golpeo basico que cualquier slime puede hacer a un enemigo.";
		}else{
			switch(slime_type){
				case 0:
					return "Un golpeo basico de slime que puede reducir la velocidad del objetivo durante un turno.";
				case 1:
					return "Un golpeo basico de slime que puede reducir el ataque del objetivo durante un turno.";
				case 2:
					return "Un golpeo basico de slime que puede reducir la defensa del objetivo durante un turno.";
				case 3:
					return "Un golpeo basico de slime que puede sellar al objetivo durante un turno.";
			}
		}
		return "false";
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
