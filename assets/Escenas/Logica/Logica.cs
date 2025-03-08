using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Logica : Node2D
{

    public struct Listas
    {
        public List<Entidades> Enemigos;
        public List<Entidades> Aliados;

        public Listas(List<Entidades> Ene, List<Entidades> Ali)
        {
            this.Enemigos = Ene;
            this.Aliados = Ali;
        }
    }
    Listas listas = new Listas(new List<Entidades>(), new List<Entidades>());
    //List<Entidades> Enemigos = new List<Entidades>();
    List<Entidades> Turnos = new List<Entidades>(); //cuando se llame para el turno se pasan las listas
    //List<Entidades> Aliados = new List<Entidades>();

    SlimeInocente prueba = new SlimeInocente(1,1,1,1,1,1,1,1, 10, 1);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ini_turno()
	{
        listas.Enemigos.Add(prueba);
        for (int i = 0; i < listas.Aliados.Count; i++)
        {
            Turnos.Add(listas.Aliados[i]);
        }
        for (int i = 0; i < listas.Enemigos.Count; i++)
		{
			Turnos.Add(listas.Enemigos[i]);
		}

        Turnos = Turnos.OrderBy(o=>o.Velocidad).ToList();

        /*Turnos.Sort(delegate (Entidades x, Entidades y)  // no esta bien hay que arreglarlo
        {   
            if (x.Velocidad < y.Velocidad) return 0;
            else if (x.Velocidad > y.Velocidad) return -1;
            else return 1;
        });*/
    }

    public List<Entidades> getListAliddos()
    {
        return listas.Aliados;
    }
    public List<Entidades> getListEnemigos()
    {
        return listas.Enemigos;
    }

    public void play_Batalla()
    {
        while (listas.Enemigos.Count != 0 || listas.Aliados.Count != 0) {
            for(int i = 0; i < Turnos.Count; i++)
            {
                listas = Turnos[i].myTurno(listas);
                actualizar_Turnos();
            }
        }
    }

    public void actualizar_Turnos() //poco eficiente?
    {
        for( int i = 0;i < Turnos.Count; i++)
        {
            for(int x = 0; x < listas.Enemigos.Count; x++)
            {
                if(Turnos[i].ID == listas.Enemigos[x].ID) {
                    if(listas.Enemigos[x].Vida > 0)
                    {
                        Turnos[i] = listas.Enemigos[x];
                    }
                    else
                    {
                        listas.Enemigos.RemoveAt(x);
                        Turnos.RemoveAt(i);
                    }
                }
            }
            for (int x = 0; x < listas.Aliados.Count; x++)
            {
                if (Turnos[i].ID == listas.Aliados[x].ID)
                {
                    if(listas.Aliados[x].Vida > 0)
                    {
                        Turnos[i] = listas.Aliados[x];
                    }
                    else
                    {
                        listas.Aliados.RemoveAt(x);
                        Turnos.RemoveAt(i);
                    }
                }
            }
        }
    }

}
