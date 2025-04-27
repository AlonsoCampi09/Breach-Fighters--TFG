using Godot;
using System;

public partial class ChuvakanMovimiento3 : Movimiento{
	
	
	public ChuvakanMovimiento3(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 8;
		assingLevel(l);
	}
	
	public override void efecto(){
		int prob = 30;
		this.origen.passData().removeMP(coste);
		GD.Print("Alex va ha hacer su ataque especial!");
		this.hurtTargets(potencia);
		if(this.casterLevel >= this.evolucion)
			this.putEffectsOnTargets(prob, Estado.Aturdido, 1, 0);
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Mazo personal";
		}else{
			return "Mola Mazo";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		if(this.casterLevel >= this.evolucion){
			coste = 9;
			potencia = 9;
		}else{
			potencia = 8;
			coste = 9;
		}
	}
	
}
