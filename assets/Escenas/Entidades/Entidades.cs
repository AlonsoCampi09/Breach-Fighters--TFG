using Godot;
using System;
using System.Collections.Generic;
using static Logica;

public abstract partial class Entidades : Node2D
{
	public int Vida, Defensa, Velocidad, Mana, Ataque, Turno, Nivel;

	public int ID { get; set; }
    public int Buf_Atq { get; set; }
    public int Buf_Def { get; set; }
    public int Deb_Atq { get; set; }
    public int Deb_Def { get; set; }
    public int Max_Mana { get; set; }
    public int Max_Vida { get; set; }



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public abstract Listas myTurno(Listas listas);

    public abstract int attack(int ID, int stat_attack, Listas listas);
    public abstract void defense();

    public int getAtacado(int stat_ataque, int potencia, int Buf_Atq, int Deb_Atq)
	{
		int dano;
		dano = potencia + Math.Max(stat_ataque * (1 + (Buf_Atq - Deb_Atq) / 100) - this.Defensa * (1 + (this.Buf_Def - this.Deb_Def) / 100), 0);

		return dano;
	}

    public int getVida(){
		return Vida;
	}
	public void setVida(int set){
		Vida = set;
	}
	
	public int getDefensa(){
		return Defensa;
	}
	public void setDefensa(int set){
		Defensa = set;
	}
	
	public int getVelocidad(){
		return Velocidad;
	}
	public void setVelocidad(int set){
		Velocidad = set;
	}
	
	public int getMana(){
		return Mana;
	}
	public void setMana(int set){
		Mana = set;
	}
	
	public int getAtaque(){
		return Ataque;
	}
	public void setAtaque(int set){
		Ataque = set;
	}
	
	public int getTurno(){
		return Turno;
	}
	public void setTurno(int set){
		Turno = set;
	}
	
	public int getNivel(){
		return Nivel;
	}
	public void setNivel(int set){
		Nivel = set;
	}
}
