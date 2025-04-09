using Godot;
using System;

public partial class FighterEnemigos : Fighter
{
    public EnemyEntity dataE;

     public override void prepareData(int level)
     {
        // GD.Print("prepareData de enemigo");
         string name = this.Name.ToString();
         if (name.Contains("Blue_Slime"))
         {
             dataE = new SlimeInocenteData(level);
         }
         else if (name.Contains("Purple_Slime"))
         {
             dataE = new SlimeVagoData(level);
         }
         else if (name.Contains("Orange_Slime"))
         {
             dataE = new SlimeAgresivoData(level);
         }
         else if (name.Contains("Grey_Slime"))
         {
             dataE = new SlimeTristeData(level);
         }
         else
         {
             GD.Print("No deberia entrar aqui.");
         }
     }
    public override void _Ready(){
		animSprite = GetNode<AnimatedSprite2D>("Sprites");
		this.prepareFighter(1);
	}
	
	public override void _Process(double delta)
	{
	}
	
	/*public void prepareFighter(int level)
	{
		this.prepareData(level);
	}
	*/
	public override EnemyEntity passData(){
        if (dataE == null)
        {
            GD.PrintErr("DATA NULL");
        }
        return dataE;
	}

    public override async System.Threading.Tasks.Task myTrun()
    {
		//GD.Print("Truno enemigo");
        Procesar_Comportamiento((int)dataE.beh_type);
        //Timer myTimer = GetNode<Timer>("Timer");
        await ToSignal(GetTree().CreateTimer(5.0), "timeout");
      //  GD.Print("fin del timer");

        //await ToSignal(CustomSignals.Instance, nameof(CustomSignals.Instance.PassTurn));
    }

    public void Procesar_Comportamiento(int comportamiento)
    {
        Random rnd = new();
        int atacado;
        GD.Print("procesando comportamiento");
       // GD.Print("Turno de enemigo ID = " + dataE.ID);

        switch (comportamiento) // solo hacen aleatorio
        {
            case (int)EnemyEntity.Behaviour.Aleatorio: //por ahora solo ataca


                atacado = rnd.Next(Battle.allylist.Count);

                attack(Battle.allylist[atacado].data.ID);

                break;
            case (int)EnemyEntity.Behaviour.Agresivo:

                atacado = rnd.Next(Battle.allylist.Count);

                attack(Battle.allylist[atacado].data.ID);
                break;
            case (int)EnemyEntity.Behaviour.Tactico:

                atacado = rnd.Next(Battle.allylist.Count);

                attack(Battle.allylist[atacado].data.ID);
                break;
            case (int)EnemyEntity.Behaviour.Apoyo:

                atacado = rnd.Next(Battle.allylist.Count);

                attack(Battle.allylist[atacado].data.ID);
                break;
            default:
                GD.PrintErr("NO TIENE COMPORTAMIENTO");

                break;
        }
    }

    public override void attack(int ID_atacado)
    {
        int x = 0, dano;
        while (Battle.allylist[x].data.ID != ID_atacado && x < Battle.allylist.Count)
        {
            x++;
        }
        dano = Battle.allylist[x].getAtacado(dataE.TrueAttack[dataE.Level-1], 10, dataE.ATQBuf, dataE.ATQDeBuf); // hay que cambiar la potencia del ataque
        string Message = this.dataE.Name.ToString() + "ha atacado a " + Battle.allylist[x].data.Name.ToString() + "y le ha quitado " + dano + "puntos de vida";
        DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId);
        GD.Print("dano de enemigo a aliado = " + dano);

        //Battle.allylist[ID_atacado].data.Health = Battle.allylist[ID_atacado].data.Health - dano;
        GD.Print("Vida de atacado = " + Battle.allylist[x].data.Health);
    }

    public override int getAtacado(int stat_ataque, int potencia, int Buf_Atq, int Deb_Atq)
    {
        int dano;
        dano = potencia + Math.Max(stat_ataque * (1 + (Buf_Atq - Deb_Atq) / 100) - dataE.TrueDefense[dataE.Level] * (1 + (dataE.DEFBuf - dataE.DEFDeBuf) / 100), 0);
        this.dataE.Health = this.dataE.Health - dano;
        return dano;
    }
}
