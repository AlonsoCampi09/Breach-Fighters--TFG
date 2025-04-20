using Godot;
using System;

public partial class IshimondoMovimiento2 : Movimiento{
	
	
	public IshimondoMovimiento2(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int coste;
		if(this.casterLevel >= 5)
			coste = 7;
		else
			coste = 4;
		
		this.origen.passData().removeMP(coste);
		this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.DeBuffDEF,3,20);
		this.objetivos[0].ActualizarIconosEstado();
		if(this.casterLevel >= 5){
			this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.Marca_del_cazador,3,0);
			this.objetivos[0].ActualizarIconosEstado();
		}
		GD.Print("Ishimondo usa Rugido de batalla!");
	}
	public override string giveTitulo(){
		if(this.casterLevel < 5){
			return "Rugido de batalla";
		}else{
			return "Marca del cazador";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 5){
			return "Ishimondo marca a un enemigo como su presa, lo que hace que su defensa se vea reducida un 20 porciento.";
		}else{
			return "Ishimondo marca a un enemigo como su presa, lo que hace que su defensa se vea reducida un 20 porciento. Todos los ataques de Ishimondo a ese enemigo le curan un 50 porciento del daño infligido.";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		if(this.casterLevel < 5){
			if(this.origen.passData().giveMP() >= 4){
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
