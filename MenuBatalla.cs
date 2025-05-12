using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static Battle;
public partial class MenuBatalla : Control
{
	private Button attack;
	private Button special;
	private Button bag;
	private Button guard;
	private Panel actingMenu;
	private Panel specialMenu;
	private Button mov1;
	private Button mov2;
	private Button mov3;
	private Button mov4;
	private Panel selecting;
	private Button inv;
	private Button currentButton;
	
	List<Fighter> allylist;
	List<Fighter> enemieslist;

	Fighter actor;

	private int num_selec = 0;

	private bool selectingTarget;
	private Flecha flecha; // Nodo de la flecha
	private int currentTargetIndex = 0; // Índice del enemigo seleccionado
	public int ID_turno { get; set; }
	AudioStreamPlayer2D audioPlayer;
	private int all_targets_avaible = 0;
	private bool[] indexes = { false, false, false, false, false, false, false, false };
	Movimiento mov_actual;
	private int target_disposition;
	private Battle battle_access;
	private bool info;
	
	private InfoPanel panelDescripcion;
	private Label labelInfoTitle;
	private Label labelInfoDescription;
	private Label labelInfoCost;
	private Label labelInfoPotencia;
	private Label labelInfoLevelNeeded;
	private Label labelInfoLevelEvolve;
	
	public override void _Ready()
	{
		audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		audioPlayer.Bus = "SoundCue";
		attack = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Attack");
		special = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Special");
		bag = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Bag");
		guard = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Guard");
		panelDescripcion = GetNode<InfoPanel>("PanelDescriptivo");
		labelInfoTitle = GetNode<Label>("PanelDescriptivo/MarginContainer/VBoxContainer/Titulo");
		labelInfoDescription = GetNode<Label>("PanelDescriptivo/MarginContainer/VBoxContainer/Descripcion");
		labelInfoPotencia = GetNode<Label>("PanelDescriptivo/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/Potencia");
		labelInfoCost = GetNode<Label>("PanelDescriptivo/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/Coste");
		labelInfoLevelNeeded = GetNode<Label>("PanelDescriptivo/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/NivelNecesario");
		labelInfoLevelEvolve = GetNode<Label>("PanelDescriptivo/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/NivelEvolucion");
		actingMenu = GetNode<Panel>("Battle_Action");
		specialMenu = GetNode<Panel>("Special_Action");
		mov1 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov1");
		mov2 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov2");
		mov3 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov3");
		mov4 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov4");
		inv = GetNode<Button>("Selection/MarginContainer/HBoxContainer/Inv");
		selecting = GetNode<Panel>("Selection");

		flecha = GetNode<Flecha>("Flecha");
		flecha.ShowArrow(false);
		mov1.Pressed += OnMov1ButtonDown;
		mov2.Pressed += OnMov2ButtonDown;
		mov3.Pressed += OnMov3ButtonDown;
		mov4.Pressed += OnMov4ButtonDown;

		attack.Pressed += OnAttackButtonDown;
		special.Pressed += OnSpecialButtonDown;
		bag.Pressed += OnBagButtonDown;
		guard.Pressed += OnGuardButtonDown;
		
		attack.FocusEntered += _OnAttackFocusEntered;
		special.FocusEntered += _OnSpecialFocusEntered;
		bag.FocusEntered += _OnObjectFocusEntered;
		guard.FocusEntered += _OnGuardFocusEntered;
		mov1.FocusEntered += _OnMov1FocusEntered;
		mov2.FocusEntered += _OnMov2FocusEntered;
		mov3.FocusEntered += _OnMov3FocusEntered;
		mov4.FocusEntered += _OnMov4FocusEntered;
		
		attack.FocusExited += _OnButtonFocusExited;
		guard.FocusExited += _OnButtonFocusExited;
		mov1.FocusExited += _OnButtonFocusExited;
		mov2.FocusExited += _OnButtonFocusExited;
		mov3.FocusExited += _OnButtonFocusExited;
		mov4.FocusExited += _OnButtonFocusExited;

		actingMenu.Visible = true;
		specialMenu.Visible = false;
		attack.GrabFocus();
		panelDescripcion.Visible = false;
		selectingTarget = false;
		selecting.Visible= false;
		info = false;
    }
    public void makeMenuVisible(Fighter f)
	{
		GD.Print("MakingMenuVisible...");
		this.prepareTitles(f);
		this.ChangeMenu(0);
	}

	public void prepareTitles(Fighter f)
	{
		Entity dataf = f.passData();
		dataf.mov1.assignCaster(f);
		dataf.mov2.assignCaster(f);
		dataf.mov3.assignCaster(f);
		dataf.mov4.assignCaster(f);
		dataf.atqBasico.assignCaster(f);
		dataf.defBasico.assignCaster(f);
		attack.Text = dataf.atqBasico.giveTitulo();
		if (dataf.mov1.enoughMana())
		{
			mov1.Text = dataf.mov1.giveTitulo();
			mov1.Disabled = false;
		}
		else
		{
			mov1.Text = "Insuficiente maná";
			mov1.Disabled = true;
		}
		if (dataf.mov2.enoughMana())
		{
			mov2.Text = dataf.mov2.giveTitulo();
			mov2.Disabled = false;
		}
		else
		{
			mov2.Text = "Insuficiente maná";
			mov2.Disabled = true;
		}
		if (dataf.mov3.moveIsAvailable())
		{
			if (dataf.mov3.enoughMana())
			{
				mov3.Text = dataf.mov3.giveTitulo();
				mov3.Disabled = false;
			}
			else
			{
				mov3.Text = "Insuficiente maná";
				mov3.Disabled = true;
			}
		}
		else
		{
			mov3.Text = "NIVEL 2";
			mov3.Disabled = true;
		}
		if (dataf.mov4.moveIsAvailable())
		{
			if (dataf.mov4.enoughMana())
			{
				mov4.Text = dataf.mov4.giveTitulo();
				mov4.Disabled = false;
			}
			else
			{
				mov4.Text = "Insuficiente maná";
				mov4.Disabled = true;
			}
		}
		else
		{
			mov4.Text = "NIVEL 3";
			mov4.Disabled = true;
		}
		if (dataf.defBasico.moveIsAvailable())
		{
			guard.Text = dataf.defBasico.giveTitulo();
		}
		else
		{
			guard.Text = "BLOQUEADO";
			guard.Disabled = true;
		}
		actor = f;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_cancel"))
		{
			ChangeMenu(0);
		}
		if (Input.IsActionPressed("Info_Input"))
		{
			GD.Print("INFO");
			ShowDesciptionPanel();
		}
		/*if (this.selectingTarget)
		{
			// Cambiar objetivo con izquierda/derecha
			if (Input.IsActionJustPressed("ui_left"))
			{
				GD.Print("Flecha visible moviendose a la izq");
				this.ChangeTarget(-1);
			}
			else if (Input.IsActionJustPressed("ui_right"))
			{
				GD.Print("Flecha visible moviendose a la der");
				this.ChangeTarget(1);
			}
			// Confirmar selección con "ui_accept"
			if (Input.IsActionJustPressed("ui_accept"))
			{
				this.ConfirmTarget();
			}
		}*/
		if (this.selectingTarget && target_disposition != 3)
		{
			// Cambiar objetivo con izquierda/derecha
			if (Input.IsActionJustPressed("ui_left"))
			{
				GD.Print("Flecha visible moviendose a la izq");
				this.ChangeTarget(-1);
			}
			else if (Input.IsActionJustPressed("ui_right"))
			{
				GD.Print("Flecha visible moviendose a la der");
				this.ChangeTarget(1);
			}
			else if (Input.IsActionJustPressed("ui_up"))
			{
				GD.Print("Flecha visible moviendose a la der");
				this.ChangeTarget(1);
			}
		}

		if (!DisplayServer.TtsIsSpeaking() && !CustomSignals.pausado)
		{

			if (!actingMenu.Visible && !this.selectingTarget && !specialMenu.Visible && !Battle.esperando)
			{
				GD.Print("ChangeMenu(0)");
				ChangeMenu(0);
			}
			if (!this.selectingTarget && !specialMenu.Visible && !Battle.esperando)
			{
				actingMenu.Visible = true;
			}
            if (info)
            {
                info = false;
            }
        }
		else
		{
           // actingMenu.Visible = false;

            if (!info)
			{
                actingMenu.Visible = false;

            }
        }

	}

	private void ChangeMenu(int c)
	{
		switch (c)
		{
			case -1:
				actingMenu.Visible = false;
				specialMenu.Visible = false;
				selecting.Visible = false;
				flecha.ShowArrow(false);
				this.selectingTarget = false;
				break;
			case 0:
				actingMenu.Visible = true;
				specialMenu.Visible = false;
				selecting.Visible = false;
				flecha.ShowArrow(false);
				this.selectingTarget = false;
				attack.GrabFocus();
				actor.passData().atqBasico.erraseTarget();
				actor.passData().mov1.erraseTarget();
				actor.passData().mov2.erraseTarget();
				actor.passData().mov3.erraseTarget();
				actor.passData().mov4.erraseTarget();
				foreach (Fighter f in enemieslist)
				{
					f.DetenerParpadeo();
				}
				for (int i = 0; i < indexes.Length; i++)
				{
					indexes[i] = false;
				}
				break;
			case 1:
				specialMenu.Visible = true;
				actingMenu.Visible = false;
				selecting.Visible = false;
				flecha.ShowArrow(false);
				this.selectingTarget = false;
				mov1.GrabFocus();
				break;
			case 2:
				actingMenu.Visible = false;
				specialMenu.Visible = false;
				selecting.Visible = true;
				selecting.Modulate = new Color(1, 1, 1, 0); // Hacerlo invisible
				inv.GrabFocus();
				this.StartTargetSelection();
				break;
		}   
	}

	private void OnAttackButtonDown()
	{
		GD.Print("Atacar");
		prepareVariablesForAttack(0);
		ChangeMenu(2);
		
	}

	private void OnSpecialButtonDown()
	{
		GD.Print("Especial");
		ChangeMenu(1);
	}

	private void OnBagButtonDown()
	{
		GD.Print("Bolsa");
	}

	private void OnGuardButtonDown()
	{
		GD.Print("Guardia");
		if (prepareVariablesForAttack(5) == true)
			ChangeMenu(2);

	}

	private void OnMov1ButtonDown()
	{
		GD.Print("Especial1");
		if (prepareVariablesForAttack(1) == true)
			ChangeMenu(2);
	}

	private void OnMov2ButtonDown()
	{
		GD.Print("Especial2");
		if (prepareVariablesForAttack(2) == true)
			ChangeMenu(2);
	}

	private void OnMov3ButtonDown()
	{
		GD.Print("Especial3");
		if (prepareVariablesForAttack(3) == true)
			ChangeMenu(2);
	}

	private void OnMov4ButtonDown()
	{
		GD.Print("Especial4");
		if (prepareVariablesForAttack(4) == true)
			ChangeMenu(2);
	}

	private void OnInvButtonDown()
	{
		GD.Print("Target CONFIRM");
		this.ConfirmTarget();
	}

	private bool prepareVariablesForAttack(int s)
	{
		switch (s)
		{
			case 0:
				mov_actual = actor.passData().atqBasico;
				break;
			case 1:
				mov_actual = actor.passData().mov1;
				break;
			case 2:
				mov_actual = actor.passData().mov2;
				break;
			case 3:
				mov_actual = actor.passData().mov3;
				break;
			case 4:
				mov_actual = actor.passData().mov4;
				break;
			case 5:
				mov_actual = actor.passData().defBasico;
				break;
		}
		mov_actual.assignCaster(actor);
		if (!mov_actual.enoughMana())
			return false;
		target_disposition = mov_actual.whoAffects();
		if (target_disposition == 0)
			GD.Print("target_disposition = ALLY");
		else if (target_disposition == 1)
			GD.Print("target_disposition = ENEMY");
		else if (target_disposition == 2)
			GD.Print("target_disposition = BOTH");
		else if (target_disposition == 3)
			GD.Print("target_disposition = SELF");
		else
			GD.PrintErr("target_disposition = IS WRONG!!!");
		num_selec = mov_actual.num_objetivos;
		if (num_selec > enemieslist.Count)
			num_selec = enemieslist.Count;
		return true;
	}

	// Funciones de respuesta a enfoque en botones 
	private void _OnAttackFocusEntered()
	{
		var correctSound = GD.Load("res://assets/Sounds/attack_sound.mp3") as AudioStream;
		currentButton = attack;
		audioPlayer.Stream = correctSound;
		audioPlayer.Play();

	}

	private void _OnSpecialFocusEntered()
	{
		var correctSound = GD.Load("res://assets/Sounds/magic_sound.mp3") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		audioPlayer.Stream = correctSound;
		audioPlayer.Play();
	}

	private void _OnObjectFocusEntered()
	{
		var correctSound = GD.Load("res://assets/Sounds/inventory_sound.mp3") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		currentButton = special;
		audioPlayer.Stream = correctSound;
		audioPlayer.Play();
	}

	private void _OnGuardFocusEntered()
	{
		var correctSound = GD.Load("res://assets/Sounds/shield_sound.mp3") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		currentButton = guard;
		audioPlayer.Stream = correctSound;
		audioPlayer.Play();
	}
	private void _OnMov1FocusEntered()
	{
	//	var correctSound = GD.Load("res://assets/Sounds/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		currentButton = mov1;
	//	audioPlayer.Stream = correctSound;
        //audioPlayer.Play();
        if (CustomSignals.activado)
        {
            DisplayServer.TtsStop();

            DisplayServer.TtsSpeak(mov1.Text, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }

    }
    private void _OnMov2FocusEntered()
	{
	//	var correctSound = GD.Load("res://assets/Sounds/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		currentButton = mov2;
	//	audioPlayer.Stream = correctSound;
        //audioPlayer.Play();
        if (CustomSignals.activado)
        {
            DisplayServer.TtsStop();

            DisplayServer.TtsSpeak(mov2.Text, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
	private void _OnMov3FocusEntered()
	{
	//	var correctSound = GD.Load("res://assets/Sounds/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		currentButton = mov3;
		//audioPlayer.Stream = correctSound;
        //audioPlayer.Play();
        if (CustomSignals.activado)
        {
            DisplayServer.TtsStop();

            DisplayServer.TtsSpeak(mov3.Text, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
	private void _OnMov4FocusEntered()
	{
	//	var correctSound = GD.Load("res://assets/Sounds/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav") as AudioStream;
	   // var audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		currentButton = mov4;
		//audioPlayer.Stream = correctSound;
        //audioPlayer.Play();

        if (CustomSignals.activado)
        {
            DisplayServer.TtsStop();

            DisplayServer.TtsSpeak(mov4.Text, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }
    }
	
	private void _OnButtonFocusExited(){
		if (panelDescripcion.Visible)
		{
            DisplayServer.TtsStop();
        }

		panelDescripcion.Visible = false;
		currentButton = null;
	}
	

	public void receiveLists(List<Fighter> ene, List<Fighter> ally, Battle battle)
	{
		this.allylist = ally;
		this.enemieslist = ene;
		this.battle_access = battle;

	}

	private void StartTargetSelection()
	{

		this.selectingTarget = true;
		// Seleccionar el primer enemigo por defecto
		currentTargetIndex = 0;
		switch (target_disposition)
		{
			case 0:
				if (this.allylist.Count == 0)
				{
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				all_targets_avaible = this.allylist.Count;
				break;
			case 1:
				if (this.enemieslist.Count == 0)
				{
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				all_targets_avaible = this.enemieslist.Count;
				break;
			case 2:
				if (this.allylist.Count == 0)
				{
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				if (this.enemieslist.Count == 0)
				{
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				all_targets_avaible = this.allylist.Count + this.enemieslist.Count;
				break;
			case 3:
				while (this.allylist[currentTargetIndex] != actor)
					currentTargetIndex++;
				GD.Print($"{this.allylist[currentTargetIndex].Name} SELF");
				break;
		}
		this.MoveArrowToCurrentTarget();
	}

	private void ChangeTarget(int direction)
	{
		switch (target_disposition)
		{
			case 0:
				this.allylist[currentTargetIndex].DetenerParpadeo();
				break;
			case 1:
				this.enemieslist[currentTargetIndex].DetenerParpadeo();
				break;
			case 2:
				if (currentTargetIndex >= this.enemieslist.Count)
				{
					this.allylist[currentTargetIndex - this.enemieslist.Count].DetenerParpadeo();
				}
				else
				{
					this.enemieslist[currentTargetIndex].DetenerParpadeo();
				}
				break;
			default:
				break;
		}
		currentTargetIndex = (currentTargetIndex + direction + all_targets_avaible) % all_targets_avaible;
		while (indexes[currentTargetIndex])
		{
			if (direction > 0)
			{
				currentTargetIndex++;
			}
			else
			{
				currentTargetIndex--;
			}
			if (currentTargetIndex < 0)
			{
				currentTargetIndex = all_targets_avaible - 1;
			}
			else if (currentTargetIndex >= all_targets_avaible)
			{
				currentTargetIndex = 0;
			}
		}
		MoveArrowToCurrentTarget();
	}

	private void MoveArrowToCurrentTarget()
	{
		switch (target_disposition)
		{
			case 0:
			case 3:
				if (this.enemieslist.Count == 0) return;
				Fighter selectedAlly = this.allylist[currentTargetIndex];
				selectedAlly.Parpadear();
				flecha.MoveArrow(selectedAlly.GetPosition() + new Vector2(0, 100)); // Ajustar la posición
				flecha.ShowArrow(true);
				DisplayServer.TtsStop();

				if (CustomSignals.activado)
				{
					CustomSignals.Instance.repetir = selectedAlly.passData().Name;
					DisplayServer.TtsSpeak(selectedAlly.passData().Name, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
				}
				break;
			case 1:
				if (this.enemieslist.Count == 0) return;
				Fighter selectedEnemy = this.enemieslist[currentTargetIndex];
				selectedEnemy.Parpadear();
				flecha.MoveArrow(selectedEnemy.GetPosition() + new Vector2(0, 100)); // Ajustar la posición
				flecha.ShowArrow(true);
				DisplayServer.TtsStop();

				if (CustomSignals.activado)
				{
					CustomSignals.Instance.repetir = selectedEnemy.passData().Name;
					DisplayServer.TtsSpeak(selectedEnemy.passData().Name, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
				}
				break;
			case 2:
				Fighter selectedFighter;
				if (currentTargetIndex >= this.enemieslist.Count)
				{
					selectedFighter = this.allylist[currentTargetIndex - this.enemieslist.Count];
				}
				else
				{
					selectedFighter = this.enemieslist[currentTargetIndex];
				}
				selectedFighter.Parpadear();
				flecha.MoveArrow(selectedFighter.GetPosition() + new Vector2(0, 100)); // Ajustar la posición
				flecha.ShowArrow(true);
				DisplayServer.TtsStop();
				if (CustomSignals.activado)
				{
					CustomSignals.Instance.repetir = selectedFighter.passData().Name;
					DisplayServer.TtsSpeak(selectedFighter.passData().Name, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
				}
				break;
			default:
				break;
		}
	}

	private void ConfirmTarget()
	{
		bool atacado = false;
	   
		Fighter confirmedTarget;
		if (target_disposition == 2)
		{
			if (currentTargetIndex >= this.enemieslist.Count)
			{
				GD.Print($"{currentTargetIndex} >= {this.allylist.Count}");
				confirmedTarget = this.allylist[currentTargetIndex - this.enemieslist.Count];
			}
			else
			{
				GD.Print($"{currentTargetIndex} < {this.allylist.Count}");
				confirmedTarget = this.enemieslist[currentTargetIndex];
			}
			mov_actual.addTarget(confirmedTarget);
		}
		else
		{
			if (target_disposition == 1)
			{
				if (mov_actual.affectsAllTeam())
				{
					foreach (Fighter f in this.enemieslist)
					{
						if (!f.passData().isDead())
						{
							confirmedTarget = f;
							mov_actual.addTarget(confirmedTarget);
						}
					}
				}
				else
				{
					confirmedTarget = this.enemieslist[currentTargetIndex];
					mov_actual.addTarget(confirmedTarget);
				}
			}
			else
			{
				if (mov_actual.affectsAllTeam())
				{
					foreach (Fighter f in this.allylist)
					{
						if (!f.passData().isDead())
						{
							confirmedTarget = f;
							mov_actual.addTarget(confirmedTarget);
						}
					}
				}
				else
				{
					confirmedTarget = this.allylist[currentTargetIndex];
					mov_actual.addTarget(confirmedTarget);
				}
			}
		}
		num_selec--;
		if (num_selec == 0 || mov_actual.affectsAllTeam())
		{
			foreach (Fighter f in mov_actual.objetivos)
			{
				f.DetenerParpadeo();
				GD.Print($"¡Enemigo {f.Name} seleccionado!");
			}
			selectingTarget = false;
			flecha.ShowArrow(false);
			ChangeMenu(-1);
			battle_access.prepareDialog(actor, mov_actual);

			/*
			mov_actual.efecto();
			mov_actual.erraseTarget();
			selectingTarget = false;
			flecha.ShowArrow(false);
			ChangeMenu(0);
			*/
		}
		else
		{
			indexes[currentTargetIndex] = true;
			this.ChangeTarget(1);
		}
		string name = actor.data.Name.ToString();
		if (name.Contains("Chuvakan") && !atacado && num_selec == 0)
		{
			CustomSignals.Instance.EmitSignal(nameof(CustomSignals.PassTurn1));
			atacado = true;

		}
		else if (name.Contains("Cassandra") && !atacado && num_selec == 0)
		{

			CustomSignals.Instance.EmitSignal(nameof(CustomSignals.PassTurn2));
			atacado = true;

		}
		else if (name.Contains("bils") && !atacado && num_selec == 0)
		{
			GD.Print("bils senal");
			CustomSignals.Instance.EmitSignal(nameof(CustomSignals.PassTurn3));
			atacado = true;

		}
		else if (name.Contains("Ishimondo") && !atacado && num_selec == 0)
		{

			CustomSignals.Instance.EmitSignal(nameof(CustomSignals.PassTurn4));
			atacado = true;

		}

	}
	public void ShowDesciptionPanel(){

        if (currentButton == attack){
			PrepareInfo(actor.passData().atqBasico,false,true);
		}else if(currentButton == guard){
			PrepareInfo(actor.passData().defBasico,false,false);
		}else if(currentButton == mov1){
			PrepareInfo(actor.passData().mov1,true,true);
		}else if(currentButton == mov2){
			PrepareInfo(actor.passData().mov2,true,true);
		}else if(currentButton == mov3){
			PrepareInfo(actor.passData().mov3,true,true);
		}else if(currentButton == mov4){
			PrepareInfo(actor.passData().mov4,true,true);
		}
	}
	
	public void PrepareInfo(Movimiento s, bool c, bool p){
		string titulo = "";
		string descripcion = "";
		string cost = "";
		string potencia = "";
		string evolve = "";
		string mensaje = "";
		titulo = s.giveTitulo();
		mensaje += titulo + " ";
        descripcion = s.giveDescripcion();
        mensaje += descripcion + " ";

		if (c) { 
			cost = $"Coste: {cost}{s.giveCost()} Maná";
            mensaje += cost + " ";
        }
		if (p)
		{
			potencia = $"Potencia: {potencia}{s.givePotencia()} POW";
            mensaje += potencia + " ";

        }
        evolve = $"Nivel evolución: {evolve}{s.giveEvolucion()}";
        mensaje += evolve;

        panelDescripcion.UpdatePanelSize();
		labelInfoTitle.Text = $"{titulo}";
		labelInfoDescription.Text = $"{descripcion}";
		labelInfoCost.Text = $"{cost}";
		labelInfoPotencia.Text = $"{potencia}";
		labelInfoLevelEvolve.Text = $"{evolve}";
		panelDescripcion.Visible = true;
        info = true;

        if (CustomSignals.activado)
        {
            DisplayServer.TtsStop();
            DisplayServer.TtsSpeak(mensaje, CustomSignals.Instance.voiceId, CustomSignals.volumenTextToSpeach, 1, CustomSignals.velocidadTextToSpeach);
        }

    }


	public void aparece()
	{
        attack.GrabFocus();

    }
}
