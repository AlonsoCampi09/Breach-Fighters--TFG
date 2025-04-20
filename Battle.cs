using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Battle : Node2D
{
	//Nodos
	private Enfrentamiento enfrentamiento;
	private static FighterTeam allies;
	private static MenuBatalla menu_de_pelea;
    private static BarrasVida barras_Vida;

    //Managers
    public static TurnManager turnManager;
	
	//Equipos y listas
	public static List<Fighter> allylist;
    public static List<Fighter> enemieslist;
	
	public override void _Ready(){
		enfrentamiento = GetNode<Enfrentamiento>("EnfrentamientoAletorio");
        allies = GetParent().GetNode<FighterTeam>("Equipo_Aliado");
        menu_de_pelea = GetNode<MenuBatalla>("Menu_Batalla");
        barras_Vida = GetNode<BarrasVida>("BarrasVida");

        allylist = allies.giveList();
		if(allylist == null){
			GD.PrintErr("allyList no tiene lista.");
		}
        enfrentamiento.giveEnfrentamiento();
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
        CustomSignals.Instance.Connect(nameof(CustomSignals.Instance.RememberA), Callable.From(RememberA), (uint)GodotObject.ConnectFlags.Deferred);
        CustomSignals.Instance.Connect(nameof(CustomSignals.Instance.RememberE), Callable.From(RememberE), (uint)GodotObject.ConnectFlags.Deferred);
    }

    public async static void Play_Batalla()
    {
        
        barras_Vida.setHealthBars();

        while (enemieslist.Count != 0 && allylist.Count != 0)
        {

			//GD.Print("enemieslist.Count = " + enemieslist.Count);
            for (int i = 0; i < turnManager.turnOrder.Count && enemieslist.Count != 0 && allylist.Count != 0; i++)
            {
                GD.Print("Turno de = " + turnManager.turnOrder[i].data.Name);
                DisplayServer.TtsSpeak("Turno de " + turnManager.turnOrder[i].data.Name, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
                menu_de_pelea.SetID_turno(turnManager.turnOrder[i].data.ID);
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
            string Message = "Batalla ganada!!";

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, CustomSignals.volumenTextToSpeach);
        }
        else
        {
            GD.Print("DERROTA");
            string Message = "Batalla perdida!!";

            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, CustomSignals.volumenTextToSpeach);
        }
        allies.setList(allylist);
        CustomSignals.Instance.EmitSignal(nameof(CustomSignals.Battlefinished));

    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void RememberA()
	{
		String mensaje = "";

		for(int i = 0;  i < allylist.Count; i++)
		{
            mensaje = mensaje + "El aliado " + allylist[i].data.Name + " tiene " + allylist[i].data.Health + 
			"puntos de vida de un maximo de " + allylist[i].data.TrueHealth[allylist[i].data.Level - 1] + 
			"y tiene " + allylist[i].data.Mana +
            "puntos de mana de un maximo de " + allylist[i].data.TrueMana[allylist[i].data.Level - 1];
        }
        DisplayServer.TtsStop();

        DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);

    }
    public void RememberE()
    {
        String mensaje = "";

        for (int i = 0; i < enemieslist.Count; i++)
        {
            int aux = (enemieslist[i].data.Health / enemieslist[i].data.TrueHealth[enemieslist[i].data.Level - 1]) * 100;
            mensaje = mensaje + "El enemigo " + enemieslist[i].data.Name + " tiene " +aux +
            " porciento de vida";
        }
        DisplayServer.TtsStop();

        DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach);
    }
}
