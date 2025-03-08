using Godot;
using System;

public abstract partial class Aliados : Entidades
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public abstract void Interfaz();

    public abstract Logica.Listas Loguica_turno(Logica.Listas listas);

    public override Logica.Listas myTurno(Logica.Listas listas)
    {
        Interfaz();
        listas = Loguica_turno(listas);
        return listas;
    }

    /*public override int attack(int ID, int stat_attack, Logica.Listas listas)
    {
        int x = 0, dano;
        while (listas.Aliados[x].ID != ID && x < listas.Aliados.Count)
        {
            x++;
        }
        dano = listas.Aliados[x].getAtacado(this.Ataque, 10); // hay que cambiar la potencia del ataque

        return dano;
    }
    public override void defense()
    {
        throw new NotImplementedException();
    }*/
}
