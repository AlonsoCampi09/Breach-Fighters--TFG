using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        BeginningMessage(); 


        barras_Vida.setHealthBars();

        while (enemieslist.Count != 0 && allylist.Count != 0)
        {

			//GD.Print("enemieslist.Count = " + enemieslist.Count);
            for (int i = 0; i < turnManager.turnOrder.Count && enemieslist.Count != 0 && allylist.Count != 0; i++)
            {
                GD.Print("Turno de = " + turnManager.turnOrder[i].data.Name);
                if (CustomSignals.activado)
                {
                    DisplayServer.TtsSpeak("Turno de " + turnManager.turnOrder[i].data.Name, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
                }
                //  menu_de_pelea.SetID_turno(turnManager.turnOrder[i].data.ID);
                if (turnManager.turnOrder[i] is FighterAliados)
                {
                    menu_de_pelea.prepareTitles(turnManager.turnOrder[i]);
                }
                GD.Print("vida = " + turnManager.turnOrder[i].data.Health);
                await turnManager.turnOrder[i].myTrun();
                barras_Vida.actualizar();

                //GD.Print("Turno pasado");
				for (int x = 0; x < turnManager.turnOrder.Count; x++)
				{
					turnManager.turnOrder[i].changeSprite();
                }
				turnManager.updateTurns();

                if(i == turnManager.turnOrder.Count)
                {
                    turnManager.updateTurnOrder();
                }
			}
        }

        if (enemieslist.Count == 0)
        {
            GD.Print("VICTORIA!!");
            EndMessage(true); 
            /*
            string Message = "Batalla ganada!!";
            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, CustomSignals.volumenTextToSpeach);
            */
        }
        else
        {
            GD.Print("DERROTA");
            EndMessage(false); 
            /*string Message = "Batalla perdida!!";
            DisplayServer.TtsSpeak(Message, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, CustomSignals.volumenTextToSpeach);*/
  
        }
        allies.setList(allylist);
        CustomSignals.Instance.EmitSignal(nameof(CustomSignals.Battlefinished));

    }




    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public static void BeginningMessage()
    {
        DisplayServer.TtsStop();

        String mensaje = "";

        //Descripcion del escenario
        mensaje = "El grupo entra en la arena del coliseo, en ella, se encuentran a  " + enemieslist.Count + " enemigos .";
        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }        //Obtenemos los personajes que siguen vivos
        List<Fighter> aux=  allylist.FindAll(IsAlive);

        if (aux.Count > 1) mensaje = "Empiezan la batalla "; 
        else mensaje = "Empieza la batalla ";

        //Listamos sus nombres
        for (int i = 0; i < aux.Count; i++)
        {
            mensaje += allylist[i].data.Name;
            if (i == aux.Count - 2) mensaje += "y ";

            else if (i == aux.Count - 1) mensaje += ".";
          
            else mensaje += " , "; 
   
        }

        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }

    public static void EndMessage(bool winner) {
        DisplayServer.TtsStop();

        if (winner) {
            String mensaje = "El grupo ha ganado la batalla, de repente, se abre la puerta hacia la siguiente sala. Avanzan hacia ella.";
            if (CustomSignals.activado)
            {
                DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
            }
        }

        else {
            String mensaje = "El grupo ha sido derrotado, el lider del coliseo les manda al area de descanso.";
            if (CustomSignals.activado)
            {
                DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
            }
        }
    }


    private static bool IsAlive(Fighter f) {
        return f.data.Health > 0; 
    }


	public void RememberA()
	{
        DisplayServer.TtsStop();

        String mensaje = "";

		for(int i = 0;  i < allylist.Count; i++)
		{
            mensaje = mensaje + "El aliado " + allylist[i].data.Name + " tiene " + allylist[i].data.Health + 
			"puntos de vida de un maximo de " + allylist[i].data.TrueHealth[allylist[i].data.Level - 1] + 
			"y tiene " + allylist[i].data.Mana +
            "puntos de mana de un maximo de " + allylist[i].data.TrueMana[allylist[i].data.Level - 1];
        }
        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
    public void RememberE()
    {
        DisplayServer.TtsStop();

        String mensaje = "";

        for (int i = 0; i < enemieslist.Count; i++)
        {
            int aux = (enemieslist[i].data.Health / enemieslist[i].data.TrueHealth[enemieslist[i].data.Level - 1]) * 100;
            mensaje = mensaje + "El enemigo " + enemieslist[i].data.Name + " tiene " +aux +
            " porciento de vida";
        }
        if (CustomSignals.activado)
        {
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
}
