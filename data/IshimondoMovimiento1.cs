using Godot;
using System;

public partial class IshimondoMovimiento1 : Movimiento{
	
	
	public IshimondoMovimiento1(int l){
		this.effectObj = Effect_Obj.Enemy;
		this.num_objetivos = 1;
		this.evolucion = 5;
		assingLevel(l);
	}
	
	public override void efecto(){
		//Logica del movimiento;
		int  potencia_total = 0, random_number, limite, garantizado, count=0, num_mayor = 5;
		bool dentro = true;
		Random rand = new Random();
		if(this.casterLevel >= this.evolucion){	
			limite = 7;
			garantizado = 3;
			
		}else{
			limite = 5;
			garantizado = 2;
		}
		this.origen.passData().removeMP(coste);
		while(dentro && count < limite-1){
			if(count >= garantizado){
				random_number = rand.Next(1, 11);
				if(random_number > num_mayor){
					potencia_total += potencia;
					if(this.casterLevel < 4)
						num_mayor++;
				}else
					dentro = false;
			}else
				potencia_total += potencia;
			count++;
		}
		this.hurtTargets(potencia);
		GD.Print("Ishimondo usa Golpes voraces!");
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
			return "Golpes voraces";
		}else{
			return "Ráfaga voraz";
		}
	}
	public override string giveDescripcion(){
		if(this.casterLevel < this.evolucion){
			return "Ishimondo ataca repetidas vesces al enemigo haciendole daño. El número de ataques puede variar.";
		}else{
			return "Ishimondo ataca repetidas vesces al enemigo haciendole daño. El número de ataques puede variar.";
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
		this.potencia = 2;
		if(this.casterLevel >= this.evolucion){
			coste = 7;
		}else{
			coste = 4;
		}
	}
	
}
