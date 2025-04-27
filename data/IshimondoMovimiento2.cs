using Godot;
using System;

public partial class IshimondoMovimiento2 : Movimiento{
	
	
	public IshimondoMovimiento2(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 6;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		if(this.casterLevel < this.evolucion){
			this.putEffectsOnTargets(100, Estado.DeBuffDEF, 3, 20);
		}else{
			this.putEffectsOnTargets(100, Estado.DeBuffDEF, 3, 20);
			this.putEffectsOnTargets(100, Estado.Marca_del_cazador, 3, 0);
		}
		
		GD.Print("Ishimondo usa Rugido de batalla!");
	}
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Rugido de batalla";
		}else{
			return "Marca del cazador";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		if(this.casterLevel >= this.evolucion){
			coste = 8;
		}else{
			coste = 5;
		}
	}
	
}
