using Godot;
using System;
using System.Linq;
using System.Collections.Generic; 

public partial class ChuvakanMovimientoBasico : Movimiento{
	
	
	public ChuvakanMovimientoBasico(int l) {
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 7;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		GD.Print("Chuvakan va ha hacer su ataque basico!");
		this.hurtTargets(potencia);
		if(this.casterLevel >= this.evolucion)
			putEffectsOnTargets(0,Estado.Creacion,1,0);
	}
	public override void putEffectsOnTargets(double proba, Estado e, int dur, int ptg){
		Random rand = new Random();
		int num_max = 1000, random_number;
		for(int i = 0; i < objetivos.Count; i++){
			random_number = rand.Next(0, num_max+1);
			if(random_number == 0){
				this.origen.passData().estadoManager.AplicarEstado(Estado.Aturdido,2,0);
				if(!afectados.ContainsKey(objetivos[i].passData().Name))
					afectados[objetivos[i].passData().Name] = new List<Estado>();
				afectados[objetivos[i].passData().Name].Add(Estado.Aturdido);
			}else if(random_number >= 250 && random_number <= 400){
				this.origen.passData().restoreMP((int) this.origen.passData().giveMAXMP()/2);
			}else if(random_number >= 550 && random_number <= 750){
				this.origen.passData().estadoManager.AplicarEstado(Estado.Regeneracion,2,0);
				if(!afectados.ContainsKey(objetivos[i].passData().Name))
					afectados[objetivos[i].passData().Name] = new List<Estado>();
				afectados[objetivos[i].passData().Name].Add(Estado.Regeneracion);
			}
		}
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Golpe Improvisado";
		}else{
			return "Enfocarse";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Un golpe basico que hace daño a un enemigo.";
		}else{
			return "Un golpe basico que hace daño a un enemigo. Puede tener un efecto secundario.";
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
		coste = 0;
		if(this.casterLevel >= this.evolucion){
			potencia = 7;
		}else{
			potencia = 7;
		}
		
	}
	
	
}
