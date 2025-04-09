using Godot;
using System;


public abstract partial class Moviento{
	
	public enum Effect_Obj {Ally, Enemy}
	
	public int dmg_origin { get; protected set; }
	public Effect_Obj effectObj { get; protected set; }
	
	public int[] objetivos;
	public int origen;
	
	public abstract void efecto();
	
	public virtual string giveTitulo(int level){
		return "";
	}
	public virtual string giveDescripcion(int level){
		return "";
	}
	
}
