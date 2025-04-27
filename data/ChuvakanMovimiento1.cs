using Godot;
using System;
using System.Linq;
using System.Collections.Generic; 

public partial class ChuvakanMovimiento1 : Movimiento{
	public ChuvakanMovimiento1(int l) {
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
		this.evolucion = 4;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		this.putEffectsOnTargets(100, Estado.BuffDMG, 1, 20);
		this.putEffectsOnTargets(100, Estado.BuffDEF, 1, 20);
		GD.Print("Chuvakan usa Motivación!");
	}
	public override void putEffectsOnTargets(double proba, Estado e, int dur, int ptg){
		Random rand = new Random();
		int num_max = 100, actual_prob, random_number;
		double aux = proba;
		while(aux < 1){
			aux *= 10;
			num_max *= 10;
		}
		actual_prob = (int) aux;
		for(int i = 0; i < objetivos.Count; i++){
			random_number = rand.Next(1, num_max+1);
			if(actual_prob + random_number > num_max){
				this.objetivos[i].passData().estadoManager.AplicarEstado(e,dur,ptg);
				this.objetivos[i].ActualizarIconosEstado();
				if(!afectados.ContainsKey(objetivos[i].passData().Name))
					afectados[objetivos[i].passData().Name] = new List<Estado>();
				afectados[objetivos[i].passData().Name].Add(e);
				if(this.casterLevel >= this.evolucion){	
					this.origen.passData().estadoManager.AplicarEstado(e,dur+1,ptg);
					this.origen.ActualizarIconosEstado();
					afectados[origen.passData().Name].Add(e);
				}
				
			}
		}
		
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Motivación";
		}else{
			return "Motivación inspiradora";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Alex motiva a uno de sus compañeros lo que aumenta el ataque y la defensa en un 20%.";
		}else{
			return "Alex motiva a uno de sus compañeros con confianza lo que aumenta el ataque y la defensa en un 20% tanto al aliado como a él";
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
			coste = 7;
		}else{
			coste = 5;
		}
	}
}
