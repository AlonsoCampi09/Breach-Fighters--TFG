using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Battle : Node2D
{
	//Nodos
	private Enfrentamiento enfrentamiento;
	private FighterTeam allies;
	private static MenuBatalla menu_de_pelea;
    private static BarrasVida barras_Vida;

    //Managers
    public static TurnManager turnManager;
	
	//Equipos y listas
	public static List<Fighter> allylist;
    public static List<Fighter> enemieslist;
	
	public override void _Ready(){
		enfrentamiento = GetNode<Enfrentamiento>("EnfrentamientoAletorio");
		allies = GetNode<FighterTeam>("Equipo_Aliado");
		menu_de_pelea = GetNode<MenuBatalla>("Menu_Batalla");
        barras_Vida = GetNode<BarrasVida>("BarrasVida");

        allylist = allies.giveList();
		if(allylist == null){
			GD.PrintErr("allyList no tiene lista.");
		}
		enemieslist = enfrentamiento.giveList();
		if(enemieslist == null){
			GD.PrintErr("enemiesList no tiene lista.");
		}
		
		GD.Print("<Printing Entity NAMES/LEVEL>");
		GD.Print("--PLAYER'S TEAM--");
		foreach (Fighter f in allylist){
			Entity c = f.passData();
			GD.Print("NAME: " + c.Name);
			GD.Print("LEVEL: " + c.Level);
			GD.Print("*****************************");
		}
		GD.Print("---------------------------");
		GD.Print("--ENEMIES'S TEAM--");
		foreach (Fighter f in enemieslist){
			Entity c = f.passData();
			GD.Print("NAME: " + c.Name);
			GD.Print("LEVEL: " + c.Level);
			GD.Print("*****************************");
		}
		
		turnManager = new TurnManager(allylist, enemieslist);
		menu_de_pelea.receiveLists(enemieslist,allylist);
		Play_Batalla();

    }

    public async static void Play_Batalla()
    {
        
        barras_Vida.setHealthBars();

        while (enemieslist.Count != 0 && allylist.Count != 0)
        {

			GD.Print("enemieslist.Count = " + enemieslist.Count);
            for (int i = 0; i < turnManager.turnOrder.Count && enemieslist.Count != 0 && allylist.Count != 0; i++)
            {
				if(turnManager.turnOrder[i].data == null)
				{
                    EnemyEntity DataE = (EnemyEntity)turnManager.turnOrder[i].passData();

                    GD.Print("Turno de = " + DataE.Name);
                    DisplayServer.TtsSpeak("Turno de " + DataE.Name, CustomSignals.Instance.voiceId);
                    menu_de_pelea.SetID_turno(DataE.ID);
                }
				else
				{
                    GD.Print("Turno de = " + turnManager.turnOrder[i].data.Name);
                    DisplayServer.TtsSpeak("Turno de " + turnManager.turnOrder[i].data.Name, CustomSignals.Instance.voiceId);
                    menu_de_pelea.SetID_turno(turnManager.turnOrder[i].data.ID);
                }				
                await turnManager.turnOrder[i].myTrun();
                barras_Vida.actualizar();

                //GD.Print("Turno pasado");
				for (int x = 0; x < turnManager.turnOrder.Count; x++)
				{
					turnManager.turnOrder[i].changeSprite();
                }
				turnManager.updateTurns();
			}
        }

        if (enemieslist.Count == 0)
        {
            GD.Print("VICTORIA!!");

        }
        else
        {
            GD.Print("DERROTA");

        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
