using Godot;
using System;

public partial class IshimondoMovimiento4 : Movimiento{
	
	
	public IshimondoMovimiento4(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 7;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		this.origen.passData().removeMP(coste);
		this.hurtTargets(potencia);
		this.putEffectsOnTargets(100,Estado.Sangrado,1,0);
		//this.objetivos[0].passData().estadoManager.AplicarEstado(Estado.Sangrado,1,0);
		//this.objetivos[0].ActualizarIconosEstado();
		GD.Print("Ishimondo usa Carantoña!");
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
			return "Carantoña salvaje";
		}else{
			return "Golpe furri";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Ishimondo usa sus garras con fiereza lo que le deja sangrando.";
		}else{
			return "Ishimondo usa sus garras con toda sus ganas lo que le deja sangrando.";
		}
	}
	public override bool affectsAllTeam(){
		return false;
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
			coste = 11;
			this.potencia = 8;
		}else{
			coste = 9;
			this.potencia = 6;
		}
	}
	
}
