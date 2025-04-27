using Godot;
using System;
using System.Linq;
using System.Collections.Generic; 

public partial class ChuvakanMovimiento4 : Movimiento{
	
	
	public ChuvakanMovimiento4(int l){
		this.effectObj = Effect_Obj.Ally;
		this.num_objetivos = 1;
		this.evolucion = 9;
		assingLevel(l);
	}
	
	public override void efecto(){
		this.origen.passData().removeMP(coste);
		GD.Print("Alex va ha hacer su ataque especial!");
		putEffectsOnTargets(100,Estado.BuffDMG,1,10);
		putEffectsOnTargets(100,Estado.Creacion,1,0);
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
			}
		}
		this.origen.passData().estadoManager.AplicarEstado(e,dur+1,0);
		this.origen.ActualizarIconosEstado();
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Lluvia de ideas";
		}else{
			return "Dibujo compartido";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
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
	public override void assingLevel(int l){
		this.casterLevel = l;
		if(this.casterLevel >= this.evolucion){
			coste = 13;
		}else{
			coste = 9;
		}
	}
	
}
