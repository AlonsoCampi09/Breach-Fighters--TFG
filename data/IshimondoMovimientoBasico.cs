using Godot;
using System;

public partial class IshimondoMovimientoBasico : Movimiento{
	
	
	public IshimondoMovimientoBasico(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 3;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		GD.Print("Ishimondo va ha hacer su ataque basico!");
		this.hurtTargets(potencia);
		if(this.casterLevel >= this.evolucion){
			this.putEffectsOnTargets(100, Estado.Sangrado, 3, 50);
		}
	}
	
	public override int hurtTargets(int p){
		int formula = 0, dañoBufado,  defensaBufado, ATQOrigen, DEFOrigen, f1, f2, vida_robada = 0;
		float porcentajeATQ, porcentajeDEF;
		
		porcentajeATQ = 1  + (origen.passData().giveDMGBuf() - origen.passData().giveDMGDeBuf()) / 100;
		ATQOrigen = origen.passData().giveDMG();
		dañoBufado = (int) (ATQOrigen * porcentajeATQ);
		f1 = ATQOrigen + dañoBufado;
		
		for(int i = 0; i < objetivos.Count; i++){
			porcentajeDEF  = 1  + (objetivos[i].passData().giveDEFBuf() - objetivos[i].passData().giveDEFDeBuf()) / 100;
			DEFOrigen = objetivos[i].passData().giveDEF();
			defensaBufado = (int) (DEFOrigen * porcentajeDEF);
			f2 = DEFOrigen + defensaBufado;
			formula = Math.Max(1,p+f1-f2);
			if(objetivos[i].passData().estadoManager.TieneEstado(Estado.Marca_del_cazador)){
				vida_robada += (int) (Math.Max(1,formula/2));
			}
			objetivos[i].ReceiveDamage(formula);
		}
		origen.passData().restoreHP(vida_robada);
		return formula;
	}
	public override string giveTitulo(){
		if(this.casterLevel < this.evolucion){
			return "Arañazo";
		}else{
			return "Mordisco";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Ishimondo se abalanza delante de un enemigo para arañarlo haciendole daño.";
		}else{
			return "Ishimondo se abalanza delante de un enemigo y le muerde fuertemente haciendo que este sangre.";
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
			potencia = 8;
		}else{
			potencia = 9;
		}
		
	}
	
}
