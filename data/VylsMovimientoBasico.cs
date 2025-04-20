using Godot;
using System;

public partial class VylsMovimientoBasico : Movimiento{
	
	
	
	public VylsMovimientoBasico(){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int potencia;
		potencia = 8 + this.casterLevel;
		
		GD.Print("Vyls va ha hacer su ataque basico!");
		this.hurtTargets(potencia);
	}
	
	public virtual int hurtTargets(int p){
		int formula = 0, dañoBufado,  defensaBufado, ATQOrigen, DEFOrigen, f1, f2;
		float porcentajeATQ, porcentajeDEF;
		
		porcentajeATQ = 1  + (origen.passData().giveDMGBuf() - origen.passData().giveDMGDeBuf()) / 100;
		ATQOrigen = origen.passData().giveDMG();
		dañoBufado = (int) (ATQOrigen * porcentajeATQ);
		f1 = ATQOrigen + dañoBufado;
		
		for(int i = 0; i < objetivos.Count; i++){
			if(this.casterLevel >= 4){
				f2 = 0;
			}else{
				porcentajeDEF  = 1  + (objetivos[i].passData().giveDEFBuf() - objetivos[i].passData().giveDEFDeBuf()) / 100;
				DEFOrigen = objetivos[i].passData().giveDEF();
				defensaBufado = (int) (DEFOrigen * porcentajeDEF);
				f2 = DEFOrigen + defensaBufado;
			}
			formula = Math.Max(1,p+f1-f2);
			objetivos[i].passData().removeHP(formula);
		}
		return formula;
	}
	
	public override string giveTitulo(){
		if(this.casterLevel < 4){
			return "Disparo de cañon";
		}else{
			return "Punto debil descubierto";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < 4){
			return "Un disparo del arma de Vyls que hace daño a un enemigo.";
		}else{
			return "Un disparo del arma de Vyls que hace daño a un enemigo. Este ataque ignora la defensa del objetivo.";
		}
	}
	public override bool affectsAllTeam(){
		return false;
	}
	public override bool moveIsAvailable(){
		return true;
	}
	public override bool enoughMana(){
		return true;
	}
	
}
