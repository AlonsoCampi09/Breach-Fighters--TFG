using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public partial class Battle : Node2D
{
	Battle Instancia;
	//Nodos
	private Enfrentamiento enfrentamiento;
	private static FighterTeam allies;
	private static MenuBatalla menu_de_pelea;
	private static BarrasVida barras_Vida;
	private CuadroTexto dialog;

	private AudioStreamPlayer2D soundplayer;
	//Managers
	public static TurnManager turnManager;

	//Equipos y listas
	public static List<Fighter> allylist;
	public static List<Fighter> enemieslist;
	private Movimiento movimientoUsado;
	public static bool esperando;

	public override void _Ready()
	{
		esperando = false;
		Instancia = this;
		CustomSignals.Instance.OnDialogConfirmed += continueBattle;
		enfrentamiento = GetNode<Enfrentamiento>("EnfrentamientoAletorio");
		allies = GetParent().GetNode<FighterTeam>("Equipo_Aliado");
		menu_de_pelea = GetNode<MenuBatalla>("Menu_Batalla");
		barras_Vida = GetNode<BarrasVida>("BarrasVida");
		soundplayer = GetNode<AudioStreamPlayer2D>("SoundBattle");
		dialog = GetNode<CuadroTexto>("Dialogo");


		allylist = allies.giveList();
		if (allylist == null)
		{
			GD.PrintErr("allyList no tiene lista.");
		}
		enfrentamiento.giveEnfrentamiento();
		enemieslist = enfrentamiento.giveList();
		if (enemieslist == null)
		{
			GD.PrintErr("enemiesList no tiene lista.");
		}

		GD.Print("<Printing Entity NAMES/LEVEL>");
		GD.Print("--PLAYER'S TEAM--");
		foreach (Fighter f in allylist)
		{
			Entity c = f.passData();
			GD.Print("NAME: " + c.Name);
			GD.Print("LEVEL: " + c.Level);
			GD.Print("*****************************");
		}
		GD.Print("---------------------------");
		GD.Print("--ENEMIES'S TEAM--");
		foreach (Fighter f in enemieslist)
		{
			Entity c = f.passData();
			GD.Print("NAME: " + c.Name);
			GD.Print("LEVEL: " + c.Level);
			GD.Print("*****************************");
		}

		turnManager = new TurnManager(allylist, enemieslist);
		menu_de_pelea.receiveLists(enemieslist, allylist, this);
		CustomSignals.Instance.Connect(nameof(CustomSignals.Instance.RememberA), Callable.From(RememberA), (uint)GodotObject.ConnectFlags.Deferred);
		CustomSignals.Instance.Connect(nameof(CustomSignals.Instance.RememberE), Callable.From(RememberE), (uint)GodotObject.ConnectFlags.Deferred);
	}

	public async void Play_Batalla()
	{

		BeginningMessage();
	 //   while (DisplayServer.TtsIsSpeaking()) { }

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
					menu_de_pelea.makeMenuVisible(turnManager.turnOrder[i]);

				}
			   
				GD.Print("vida = " + turnManager.turnOrder[i].data.Health);

				await turnManager.turnOrder[i].myTrun();
				continueBattle();
				if (turnManager.turnOrder[i] is FighterAliados)
				{
					await aux_continueBattle();
				}

				barras_Vida.actualizar();

				//GD.Print("Turno pasado");
				for (int x = 0; x < turnManager.turnOrder.Count; x++)
				{
					turnManager.turnOrder[i].changeSprite();
				}
				turnManager.updateTurns();

				if (i == turnManager.turnOrder.Count)
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

	public void prepareDialog(Fighter actor, Movimiento mov_actual)
	{
		movimientoUsado = mov_actual;
		string dialogo = actor.passData().Name + " ha usado " + movimientoUsado.giveTitulo();
		if (movimientoUsado.whoAffects() != 3)
		{
			dialogo = $"{dialogo} en ";
			if (movimientoUsado.affectsAllTeam())
			{
				if (movimientoUsado.whoAffects() == 0)
					dialogo = $"{dialogo}su equipo";
				else if (movimientoUsado.whoAffects() == 1)
					dialogo = $"{dialogo}el equipo enemigo";
			}
			else
			{
				dialogo = $"{dialogo}{movimientoUsado.objetivos[0].passData().Name}";
				if (movimientoUsado.objetivos.Count > 1)
				{
					int i = 1;
					while (i < movimientoUsado.objetivos.Count - 1)
					{
						dialogo = $"{dialogo}, {movimientoUsado.objetivos[i].passData().Name}";
						i++;
					}
					dialogo = $"{dialogo} y {movimientoUsado.objetivos[i].passData().Name}";
				}
			}
		}
		dialogo = $"{dialogo}.";
		GD.Print($"Dialogo preparado: {dialogo}");
		dialog.ShowDialog(dialogo);
	   // CustomSignals.Instance.EmitSignal(nameof(CustomSignals.OnDialogRequested), dialogo);
	}
	public void prepareDialogConsecuence()
	{
		string dialogo = "";
		string affected = "";
		if (movimientoUsado.someAffected())
		{
			var entry = movimientoUsado.afectados.First();
			affected = entry.Key;
			dialogo = $"{affected} ha sido afectado por";
			List<Estado> estados = entry.Value;
			int i = 0;
			dialogo = $"{dialogo} {EstadosATexto(estados[i])}";
			i++;
			if (i < estados.Count)
			{
				while (i < estados.Count - 1)
				{
					dialogo = $"{dialogo}, {EstadosATexto(estados[i])}";
					i++;
				}
				dialogo = $"{dialogo} y {EstadosATexto(estados[i])}";
			}
		}
		dialogo = $"{dialogo}.";
		GD.Print($"Dialogo preparado: {dialogo}");
		movimientoUsado.removeAffected(affected);
		dialog.ShowDialog(dialogo);
		//CustomSignals.Instance.EmitSignal(nameof(CustomSignals.OnDialogRequested), dialogo);
	}

	public async System.Threading.Tasks.Task aux_continueBattle()
	{
		esperando = true;
		await ToSignal(CustomSignals.Instance, "OnDialogConfirmed");
		esperando = false;

	}

	public async void continueBattle()
	{
		GD.Print("continueBattle");
		movimientoUsado.efecto();
		soundplayer.Play();
		if (esperando)
		{
			GD.Print(" continueBattle esperando true");
		}
		await ToSignal(CustomSignals.Instance, "OnMoveResolved");
		CustomSignals.Instance.OnDialogConfirmed -= continueBattle;
		while (movimientoUsado.someAffected())
		{
			prepareDialogConsecuence();
			await ToSignal(CustomSignals.Instance, "OnDialogConfirmed");
		}
		CustomSignals.Instance.OnDialogConfirmed += continueBattle;

		movimientoUsado.erraseTarget();

	   // analizeBattle();
	}

	public string EstadosATexto(Estado e)
	{
		switch (e)
		{
			case Estado.BuffDMG:
				return "un potenciador de dano";
			case Estado.DeBuffDMG:
				return "una reduccion del de dano";
			case Estado.BuffDEF:
				return "un potenciador de defensa";
			case Estado.DeBuffDEF:
				return "una reduccion del de defensa";
			case Estado.BuffVEL:
				return "un potenciador de velocidad";
			case Estado.DeBuffVEL:
				return "una reduccion del de velocidad";
			case Estado.Aturdido:
				return "aturdimiento";
			case Estado.Sellado:
				return "sello magico";
			case Estado.Bloqueo:
				return "bloqueo de defensas";
			case Estado.Sangrado:
				return "sangrado";
			case Estado.Envenenado:
				return "veneno";
			case Estado.Regeneracion:
				return "regeneracion pasiva de vida";
			case Estado.Energetico:
				return "regeneracion pasiva de mana";
			case Estado.Evasion:
				return "evasion";
			case Estado.Marca_del_cazador:
				return "la marca del cazador";
			case Estado.Creacion:
				return "el potenciador de Alex";
			case Estado.Vanguardia:
				return "la proteccion de Vyls";
			default:
				return "un estado no reconocido (espera, que?)";
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public static void BeginningMessage()
	{
	   // DisplayServer.TtsStop();

		String mensaje = "";

		//Descripcion del escenario
		mensaje = "El grupo entra en la arena del coliseo, en ella, se encuentran a  " + enemieslist.Count + " enemigos .";
		if (CustomSignals.activado)
		{
			//DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
		}        //Obtenemos los personajes que siguen vivos
		List<Fighter> aux = allylist.FindAll(IsAlive);

		if (aux.Count > 1) mensaje += "Empiezan la batalla ";
		else mensaje += "Empieza la batalla ";

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

	public static void EndMessage(bool winner)
	{
	   // DisplayServer.TtsStop();

		if (winner)
		{
			String mensaje = "El grupo ha ganado la batalla, de repente, se abre la puerta hacia la siguiente sala. Avanzan hacia ella.";
			if (CustomSignals.activado)
			{
				DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
			}
		}

		else
		{
			String mensaje = "El grupo ha sido derrotado, el lider del coliseo les manda al area de descanso.";
			if (CustomSignals.activado)
			{
				DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
			}
		}
	}


	private static bool IsAlive(Fighter f)
	{
		return f.data.Health > 0;
	}


	public void RememberA()
	{
		DisplayServer.TtsStop();

		String mensaje = "";

		for (int i = 0; i < allylist.Count; i++)
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
			mensaje = mensaje + "El enemigo " + enemieslist[i].data.Name + " tiene " + aux +
			" porciento de vida";
		}
		if (CustomSignals.activado)
		{
			DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
		}
	}
}
