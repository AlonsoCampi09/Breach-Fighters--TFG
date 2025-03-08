using Godot;
using System;

public partial class Chuvakan : Aliados
{

    public Chuvakan(int Vida, int Defensa, int Velocidad, int Mana, int Ataque, int Nivel, int Id, int Max_Mana, int Max_Vida)
    {
        this.Velocidad = Vida;
        this.Defensa = Defensa;
        this.Velocidad = Velocidad;
        this.Mana = Mana;
        this.Ataque = Ataque;
        this.Nivel = Nivel;
        this.ID = Id;
        this.Buf_Atq = 0;
        this.Buf_Def = 0;
        this.Deb_Atq = 0;
        this.Deb_Def = 0;
        this.Max_Mana = Max_Mana;
        this.Max_Vida = Max_Vida;

    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


    public override void Interfaz() //se cambia la interfaz dependiendo del aliado que le toque (HOLA ALEX!!! UwU)
    {

    }

    public override Logica.Listas Loguica_turno(Logica.Listas listas) //logica del turno
    {

        //aqui cuando se elija la accion y el objetivo se cambiara en las listas y se devolveran estas mismas



        return listas;
    }

    /*public override Logica.Listas myTurno(Logica.Listas listas) // funcionalidad del turno
    {
        throw new NotImplementedException();
    }*/

    public override int attack(int ID, int stat_attack, Logica.Listas listas)
    {
        int x = 0, dano;
        while (listas.Aliados[x].ID != ID && x < listas.Aliados.Count)
        {
            x++;
        }
        dano = listas.Aliados[x].getAtacado(this.Ataque, 10, this.Buf_Atq, this.Deb_Atq); // hay que cambiar la potencia del ataque

        return dano;
    }
    public override void defense()
    {
        this.Buf_Def = 15;
        int mana_aux, mana_ganado;
        mana_ganado = this.Max_Mana / 4;
        mana_aux = mana_ganado + this.Mana;
        if (this.Max_Mana < mana_aux)
        {
            this.Mana = this.Max_Mana;
        }
        else
        {
            this.Mana = mana_aux;
        }
    }
}
