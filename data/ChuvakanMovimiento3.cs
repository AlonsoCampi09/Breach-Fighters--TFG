using Godot;
using System;

public partial class ChuvakanMovimiento3 : Movimiento{
	
	
	public ChuvakanMovimiento3(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		int potencia, coste, probabilidad = 30, random_number;
		Random rand = new Random();
		random_number = rand.Next(1, 101);
		if(this.casterLevel < 7){
			potencia = 11 + this.casterLevel;
			coste = 5;
		}else{
			potencia = 12 + this.casterLevel;
			coste = 9;
		}
		this.origen.passData().removeMP(coste);
		
		GD.Print("Alex va ha hacer su ataque especial!");
		//this.hurtTargets(potencia);
		if(random_number >= 100-probabilidad){
			this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.Aturdido,1,0);
			this.objetivos[0].ActualizarIconosEstado();
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 3){
			return "Mazo personal";
		}else{
			return "Mola Mazo";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 3){
			return "Alex dibuja su arma favorita y la hace realidad para hacer gran daño a un enemigo.";
		}else{
			return "Alex dibuja su arma favorita y la hace realidad para hacer gran daño a un enemigo. Este ataque puede aturdir al objetivo";
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
		if(this.casterLevel < 8){
			if(this.origen.passData().giveMP() >= 5){
				return true;
			}else
				return false;
		}else{
			if(this.origen.passData().giveMP() >= 9){
				return true;
			}else
				return false;
		}
	}
	
}
