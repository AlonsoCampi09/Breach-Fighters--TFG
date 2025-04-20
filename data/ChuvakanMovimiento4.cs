using Godot;
using System;

public partial class ChuvakanMovimiento4 : Movimiento{
	
	
	public ChuvakanMovimiento4(){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		int coste;
		if(this.casterLevel < 7){
			coste = 9;
		}else{
			coste = 13;
		}
		this.origen.passData().removeMP(coste);
		GD.Print("Alex va ha hacer su ataque especial!");
		for(int i = 0; i < objetivos.Count; i++){
			this.objetivos[i].passData().estadoManager.AplicarEstado(Estado.BuffDMG,1,10);
			this.objetivos[i].passData().estadoManager.AplicarEstado(Estado.Creacion,1,0);
			this.objetivos[i].ActualizarIconosEstado();
		}
		this.origen.passData().estadoManager.AplicarEstado(Estado.Creacion,2,0);
		this.origen.ActualizarIconosEstado();
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 3){
			return "Lluvia de ideas";
		}else{
			return "Dibujo compartido";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 9){
			return "Alex usa su cuaderno para otorgar una bendicion a sus compañeros. Esta bendición aumenta el ataque de todos sus aliados en un 10 porciento ademas de otorgarles Explosion creativa en sus proximos turnos.";
		}else{
			return "Alex usa su cuaderno para otorgar una bendicion a sus compañeros. Esta bendición aumenta el ataque de todos sus aliados en un 20 porciento ademas de otorgarles Explosion creativa y Ultimo estrector proximos turnos.";
		}
	}
	public override bool affectsAllTeam(){
		return true;
	}
	public override bool moveIsAvailable(){
		if(this.casterLevel < 3){
			return false;
		}else{
			return true;
		}
	}
	public override bool enoughMana(){
		if(this.casterLevel < 9){
			if(this.origen.passData().giveMP() >= 9){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 13){
				return true;
			}else
				return false;
		}
	}
	
}
