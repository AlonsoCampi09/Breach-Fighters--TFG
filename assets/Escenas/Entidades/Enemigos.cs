using Godot;
using System;
using System.Collections.Generic;
using static Logica;

public abstract partial class Enemigos : Entidades
{
    public enum Comportamiento
    {
        Aleatorio,
        Agresivo,
        Tactico,
        Apoyo
    };
    public Comportamiento comportamiento;

    List<Entidades> malos = new List<Entidades>();
    List<Entidades> buenos = new List<Entidades>();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
			

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    //public abstract int attack(int ID, int stat_attack, Listas listas);
    //public abstract void defense();

    public override Listas myTurno(Listas listas)
    {
        if(this.Vida <= 0)
        {
            listas = Procesar_Comportamiento(comportamiento, listas);
        }

        return listas;
    }

    public Listas Procesar_Comportamiento(Comportamiento comportamiento, Listas listas)
	{
		switch (comportamiento)
		{
			case Comportamiento.Aleatorio: //por ahora solo ataca
                int dano; 
                Random rnd = new();
                int ID_atacado = rnd.Next(listas.Aliados.Count);
                dano = attack(ID_atacado, this.Ataque, listas);
                listas.Aliados[ID_atacado].setVida(listas.Aliados[ID_atacado].getVida() - dano);

                break; 
			case Comportamiento.Agresivo:

            break;
			case Comportamiento.Tactico:

            break;
			case Comportamiento.Apoyo: 

			break;
			default:

			break;
		}
        return listas; // para que nos salga el error pero no tendria que estar
    }

	
}
