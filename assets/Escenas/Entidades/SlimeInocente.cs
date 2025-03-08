using Godot;
using System;

public partial class SlimeInocente : Enemigos
{

    public SlimeInocente(int Vida, int Defensa, int Velocidad, int Mana, int Ataque, int Nivel, int Id, int logica, int Max_Mana, int Max_Vida)
    {
        this.Velocidad = Vida;
        this.Defensa = Defensa;
        this.Velocidad = Velocidad;
        this.Mana = Mana;
        this.Ataque = Ataque;
        this.Nivel = Nivel;
        this.comportamiento = (Comportamiento)logica;
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
        throw new NotImplementedException();
    }
}
