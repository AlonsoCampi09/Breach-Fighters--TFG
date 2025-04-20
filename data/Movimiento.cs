using Godot;
using System;
using System.Linq;
using System.Collections.Generic;  // This is the library that includes Dictionary


public abstract partial class Movimiento{
	
	public enum Effect_Obj {Ally, Enemy, Both, Self}
	
	public Effect_Obj effectObj { get; protected set; }
	
	public List<Fighter> objetivos = new List<Fighter>();
	public Fighter origen;
	
	public int num_objetivos { get; protected set; } = 1; 
	
	public int casterLevel = 1;
	
	public abstract void efecto();
	
	public void assingCaster(Fighter caster){
		this.origen = caster;
	}
	public void assingLevel(int l){
		this.casterLevel = l;
	}
	public void addTarget(Fighter i){
		objetivos.Add(i);
	}
	public void erraseTarget(){
		while (objetivos.Count > 0){
			objetivos.RemoveAt(objetivos.Count - 1);
		}
	}
	public int whoAffects(){
		switch(effectObj){
			case Effect_Obj.Ally:
				return 0;
			case Effect_Obj.Enemy:
				return 1;
			case Effect_Obj.Both:
				return 2;
			case Effect_Obj.Self:
				return 3;
		}
		return -1;
	}
	public virtual int hurtTargets(int p){
		int formula = 0, dañoBufado,  defensaBufado, ATQOrigen, DEFOrigen, f1, f2;
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
			objetivos[i].passData().removeHP(formula);
		}
		return formula;
	}
	public virtual string giveTitulo(){
		return "";
	}
	public virtual string giveDescripcion(){
		return "";
	}
	public virtual bool affectsAllTeam(){
		return false;
	}
	public virtual bool moveIsAvailable(){
		return false;
	}
	public virtual bool enoughMana(){
		return false;
	}
	
}
