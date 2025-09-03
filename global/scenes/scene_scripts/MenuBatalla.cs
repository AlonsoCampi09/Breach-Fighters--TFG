using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MenuBatalla : Control{
	private Panel actingMenu;
	private Button attack;
	private Button special;
	private Button bag;
	private Button guard;
	private Panel specialMenu;
	private Button mov1;
	private Button mov2;
	private Button mov3;
	private Button mov4;
	private Panel selecting;
	private Button inv;
	private InfoPanel panelInfo;
	private Label labelInfoTitle;
	private Label labelInfoDescription;
	private Label labelInfoPower;
	private Label labelInfoCost;
	private Label labelInfoLevelNeeded;
	private Label labelInfoLevelEvolve;
	
	private Button currentButtonFocus;

	private List<Fighter> allyList;
	private List<Fighter> enemyList;
	public List<Fighter> allFightersList = new List<Fighter>();
	
	private Fighter actor;
	private Skill atqbas;
	private Skill guardia;
	private Skill sp1;
	private Skill sp2;
	private Skill sp3;
	private Skill sp4;
	
	private bool selectingTarget;
	private int currentTargetIndex = 0; // Índice del enemigo seleccionado
	private int num_selec = 0;
	private int all_targets_avaible = 0;
	private int indexTargetStart = 0;
	private int indexTargetEnd = 0;
	private bool[] alreadySelected = {false,false,false,false,false,false,false,false,false,false};
	private Skill mov_actual;
	private int target_disposition;
	
	private CustomSignals customSignals;
	private bool acceptingInputs = false;
	private bool canShowInfo = false;
	
	private AudioStreamPlayer2D audioPlayer;
	
	public override void _Ready(){
		actingMenu = GetNode<Panel>("Battle_Action");
		specialMenu = GetNode<Panel>("Special_Action");
		selecting = GetNode<Panel>("Selection");
		panelInfo = GetNode<InfoPanel>("Info_Panel");
		
		attack = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Attack");
		special = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Special");
		bag = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Bag");
		guard = GetNode<Button>("Battle_Action/MarginContainer/HBoxContainer/Guard");
		
		mov1 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov1");
		mov2 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov2");
		mov3 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov3");
		mov4 = GetNode<Button>("Special_Action/MarginContainer/HBoxContainer/Mov4");
		
		inv = GetNode<Button>("Selection/MarginContainer/HBoxContainer/Inv");
		audioPlayer = GetNode<AudioStreamPlayer2D>("Sonidos");
		
		labelInfoTitle = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/Title");
		labelInfoDescription = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/Description");
		labelInfoPower = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/Power");
		labelInfoCost = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/Cost");
		labelInfoLevelNeeded = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/Level_Needed");
		labelInfoLevelEvolve = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/Level_Evolve");
		
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.OnPrepareBattleMenuForFighter += PrepareMenuForFighter;
		customSignals.OnShowBattleMenu += MakeMenuVisible;
		
		attack.Pressed += OnAttackButtonPressed;
		special.Pressed += OnSpecialButtonPressed;
		bag.Pressed += OnBagButtonPressed;
		guard.Pressed += OnGuardButtonPressed;
		
		mov1.Pressed += OnMov1ButtonPressed;
		mov2.Pressed += OnMov2ButtonPressed;
		mov3.Pressed += OnMov3ButtonPressed;
		mov4.Pressed += OnMov4ButtonPressed;
		
		attack.FocusEntered += OnAttackFocusEntered;
		special.FocusEntered += OnSpecialFocusEntered;
		bag.FocusEntered += OnBagFocusEntered;
		guard.FocusEntered += OnGuardFocusEntered;
		mov1.FocusEntered += OnMov1FocusEntered;
		mov2.FocusEntered += OnMov2FocusEntered;
		mov3.FocusEntered += OnMov3FocusEntered;
		mov4.FocusEntered += OnMov4FocusEntered;
		
		attack.FocusExited += OnButtonFocusExited;
		special.FocusExited += OnButtonFocusExited;
		bag.FocusExited += OnButtonFocusExited;
		guard.FocusExited += OnButtonFocusExited;
		mov1.FocusExited += OnButtonFocusExited;
		mov2.FocusExited += OnButtonFocusExited;
		mov3.FocusExited += OnButtonFocusExited;
		mov4.FocusExited += OnButtonFocusExited;
		
		inv.Pressed += OnInvButtonPressed;
		
		actingMenu.Visible = false;
		specialMenu.Visible = false;
		selecting.Visible = false;
		
		selectingTarget = false;
		panelInfo.Visible = false;
	}
	
	public void MakeMenuVisible(){
		GD.Print("MakingMenuVisible...");
		acceptingInputs = true;
		this.ChangeMenu(0);
	}
	
	public void PrepareMenuForFighter(Fighter f, FighterTeam allies, FighterTeam enemies){
		actor = f;
		ReceiveTeams(allies,enemies);
		PrepareTitles(actor);
	}
	public void ReceiveTeams(FighterTeam allies, FighterTeam enemies){
		allyList = allies.GetFighters();
		enemyList = enemies.GetFighters();
		allFightersList = enemyList.Concat(allyList).ToList();
	}
	public void PrepareTitles(Fighter f){
		atqbas = f.AtqBasico();
		guardia = f.Guardia();
		sp1 = f.Skill1();
		sp2 = f.Skill2();
		sp3 = f.Skill3();
		sp4 = f.Skill4();
		PrepareButtons(mov1,sp1,f);
		PrepareButtons(mov2,sp2,f);
		PrepareButtons(mov3,sp3,f);
		PrepareButtons(mov4,sp4,f);
		if(guard != null){
			if(!f.IsBlocked()){
				guard.Text = "Guardia";
				guard.Disabled = false;
			}else{
				guard.Text = "BLOQUEADO";
				guard.Disabled = true;
			}
		}else{
			guard.Text = "NULL";
			guard.Disabled = true;
		}
		if(!f.IsSealed()){
			special.Text = "Especial";
			special.Disabled = false;
		}else{
			special.Text = "SELLADO";
			special.Disabled = true;
		}
		
	}
	public void PrepareButtons(Button b, Skill s, Fighter f){
		string t = "";
		bool d = false;
		if(s != null){
			if(s.MoveIsAvailable()){
				if(s.EnoughMana(f)){
					t = s.DisplayName;
				}else{
					t = "Insuficiente maná";
					d = true;
				}
			}else{
				t = $"NIVEL  {s.RequiredLevelToUnlock}";
					d = true;
			}
		}else{
			t = "NULL";
			d = true;
		}
		b.Text = t;
		b.Disabled = d;
	}
	private void PrepareVariablesForAttack(){
		target_disposition = mov_actual.WhoAffects();
		List<Fighter> listRef;
		if(target_disposition == 0){
			GD.Print("target_disposition = ALLY");
			listRef = allyList;
		}else if(target_disposition == 1){
			GD.Print("target_disposition = ENEMY");
			listRef = enemyList;
		}else if(target_disposition == 2){
			GD.Print("target_disposition = BOTH");
		}else if(target_disposition == 3){
			GD.Print("target_disposition = SELF");
		}
		else
			GD.PrintErr("target_disposition = IS WRONG!!!");
		num_selec = mov_actual.TargetsCount;
		int cont = 0, i = 0;
		foreach(Fighter f in enemyList){
			if(!f.IsDead() || !alreadySelected[i]){
				cont++;
			}
			i++;
		}
		if(num_selec > enemyList.Count)
			num_selec = enemyList.Count;
	}
	
	private void ChangeMenu(int c){
		switch (c)
		{
			case -1:
				canShowInfo = false;
				acceptingInputs = false;
				actingMenu.Visible = false;
				specialMenu.Visible = false;
				selecting.Visible = false;
				this.selectingTarget = false;
				foreach (Fighter f in enemyList){
					f.StopBlink();
				}
				for (int i = 0; i < alreadySelected.Length; i++){
					alreadySelected[i] = false;  
				}
				break;
			case 0:
				canShowInfo = true;
				actingMenu.Visible = true;
				specialMenu.Visible = false;
				selecting.Visible = false;
				this.selectingTarget = false;
				attack.GrabFocus();
				foreach (Fighter f in enemyList){
					f.StopBlink();
				}
				for (int i = 0; i < alreadySelected.Length; i++){
					alreadySelected[i] = false;  
				}
				break;
			case 1:
				canShowInfo = true;
				specialMenu.Visible = true;
				actingMenu.Visible = false;
				selecting.Visible = false;
				this.selectingTarget = false;
				mov1.GrabFocus();
				break;
			case 2:
				canShowInfo = false;
				actingMenu.Visible = false;
				specialMenu.Visible = false;
				selecting.Visible = true;
				selecting.Modulate = new Color(1, 1, 1, 0); // Hacerlo invisible
				inv.GrabFocus();
				this.StartTargetSelection();
				break;
		}
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel") && acceptingInputs){
			for(int i = 0; i < allFightersList.Count; i++){
				allFightersList[i].StopBlink();
			}
			ChangeMenu(0);
		}
		if (Input.IsActionJustPressed("custom_ui_info") && canShowInfo){
			GD.Print("Hola");
			ShowMoveInfo();
		}
		if (Input.IsActionJustPressed("custom_ui_fighter_info") && acceptingInputs && this.selectingTarget){
			TTS.StopTTS();
			TTS.PutThisInQueue(allFightersList[currentTargetIndex].FighterShowBattleInfo());
		}
		if (Input.IsActionJustPressed("custom_ui_turner_info") && acceptingInputs){
			TTS.StopTTS();
			TTS.PutThisInQueue(actor.FighterShowBattleInfo());
		}
		if (this.selectingTarget && target_disposition  != 3)
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
	}
	
	private void OnAttackButtonPressed(){
		GD.Print("Atacar");
		mov_actual = atqbas;
		PrepareVariablesForAttack();
		ChangeMenu(2);
		
	}
	private void OnSpecialButtonPressed(){
		GD.Print("Especial");
		ChangeMenu(1);
	}
	private void OnBagButtonPressed(){
		GD.Print("Bolsa");
	}
	private void OnGuardButtonPressed(){
		GD.Print("Guardia");
		mov_actual = guardia;
		PrepareVariablesForAttack();
		ChangeMenu(2);
	}
	private void OnMov1ButtonPressed(){
		GD.Print("Especial1");
		mov_actual = sp1;
		PrepareVariablesForAttack();
		ChangeMenu(2);
	}
	private void OnMov2ButtonPressed(){
		GD.Print("Especial2");
		mov_actual = sp2;
		PrepareVariablesForAttack();
		ChangeMenu(2);
	}
	private void OnMov3ButtonPressed(){
		GD.Print("Especial3");
		mov_actual = sp3;
		PrepareVariablesForAttack();
		ChangeMenu(2);
	}
	private void OnMov4ButtonPressed(){
		GD.Print("Especial4");
		mov_actual = sp4;
		PrepareVariablesForAttack();
		ChangeMenu(2);
	}
	private void OnInvButtonPressed(){
		GD.Print("Target CONFIRM");
		this.ConfirmTarget();
	}
	
	private void OnAttackFocusEntered(){
		currentButtonFocus = attack;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/attack_sound.mp3");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
	}
	private void OnSpecialFocusEntered(){
		currentButtonFocus = special;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/magic_sound.mp3");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
	}
	private void OnBagFocusEntered(){
		currentButtonFocus = bag;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/inventory_sound.mp3");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
	}
	private void OnGuardFocusEntered(){
		currentButtonFocus = guard;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/shield_sound.mp3");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
	}
	private void OnMov1FocusEntered(){
		currentButtonFocus = mov1;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
		TTS.SayThis($"{sp1.GiveTitulo()}");
	}
	private void OnMov2FocusEntered(){
		currentButtonFocus = mov2;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
		TTS.SayThis($"{sp2.GiveTitulo()}");
	}
	private void OnMov3FocusEntered(){
		currentButtonFocus = mov3;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
		TTS.SayThis($"{sp3.GiveTitulo()}");
	}
	private void OnMov4FocusEntered(){
		currentButtonFocus = mov4;
		AudioStream stream = GD.Load<AudioStream>("res://assets/sonidos/Casual Game Sounds U6/CasualGameSounds/DM-CGS-03.wav");
		audioPlayer.Stream = stream;
		audioPlayer.Play();
		TTS.SayThis($"{sp4.GiveTitulo()}");
	}
	
	private void OnButtonFocusExited(){
		currentButtonFocus = null;
		panelInfo.Visible = false;
		TTS.StopTTS();
	}
	
	
	private void StartTargetSelection(){
		this.selectingTarget = true;
		// Seleccionar el primer enemigo por defecto
		currentTargetIndex = 0;
		switch(target_disposition){
			case 0:
				if (this.allyList.Count == 0){
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				GD.Print("target_disposition = ALLY");
				all_targets_avaible = this.allyList.Count;
				indexTargetStart = this.enemyList.Count;
				indexTargetEnd = this.allFightersList.Count;
				currentTargetIndex = indexTargetStart;
				break;
			case 1:
				if (this.enemyList.Count == 0){
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				GD.Print("target_disposition = ENEMY");
				all_targets_avaible = this.enemyList.Count;
				indexTargetStart = 0;
				indexTargetEnd = this.enemyList.Count;
				currentTargetIndex = indexTargetStart;
				break;
			case 2:
				if (this.allyList.Count == 0){
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				if (this.enemyList.Count == 0){
					GD.PrintErr("No hay enemigos disponibles.");
					return;
				}
				GD.Print("target_disposition = BOTH");
				all_targets_avaible = this.allyList.Count + this.enemyList.Count;
				indexTargetStart = this.enemyList.Count;
				indexTargetEnd = this.allFightersList.Count;
				currentTargetIndex = indexTargetStart;
				break;
			case 3:
				GD.Print("target_disposition = SELF");
				all_targets_avaible = this.allyList.Count + this.enemyList.Count;
				indexTargetStart = this.enemyList.Count;
				indexTargetEnd = this.allFightersList.Count;
				currentTargetIndex = indexTargetStart;
				while(this.allFightersList[currentTargetIndex] != actor){
					currentTargetIndex++;
				}
				GD.Print($"{this.allFightersList[currentTargetIndex].GetEntityData().Name} SELF");
				break;
		}
		this.ChangeTarget(0);
	}
	private void AimToCurrentTarget(){
		if(mov_actual.AffectsAllTeam()){
			if(target_disposition == 0){
				for(int i = 0; i < allyList.Count; i++){
					if(!allyList[i].IsDead())
						allyList[i].StartBlink();
				}
			}else if(target_disposition == 1){
				for(int i = 0; i < enemyList.Count; i++){
					if(!enemyList[i].IsDead())
						enemyList[i].StartBlink();
				}
			}else{
				for(int i = 0; i < allFightersList.Count; i++){
					if(!allFightersList[i].IsDead())
						allFightersList[i].StartBlink();
				}
			}
		}
		else
			allFightersList[currentTargetIndex].StartBlink();
	}
	private void ChangeTarget(int direction){
		GD.Print($"currentTargetIndex = {currentTargetIndex}");
		GD.Print($"indexTargetEnd = {indexTargetEnd}");
		allFightersList[currentTargetIndex].StopBlink();
		currentTargetIndex = currentTargetIndex + direction;
		if(currentTargetIndex <= -1){
			currentTargetIndex = indexTargetEnd-1;
		}else if(currentTargetIndex >= indexTargetEnd){
			currentTargetIndex = indexTargetStart;
		}
		while (alreadySelected[currentTargetIndex] || allFightersList[currentTargetIndex].IsDead()){
			if (direction > 0){
				currentTargetIndex++;
			}
			else{
				currentTargetIndex--;
			}
			if (currentTargetIndex < 0){
				currentTargetIndex = indexTargetEnd-1;
			}
			else if (currentTargetIndex >= indexTargetEnd){
				currentTargetIndex = indexTargetStart;
			}
		}
		GD.Print($"{currentTargetIndex} correspode a {allFightersList[currentTargetIndex].GetEntityData().Name}");
		TTS.SayThis($"{allFightersList[currentTargetIndex].GetEntityData().Name}");
		AimToCurrentTarget();
	}
	private void ConfirmTarget(){
		if(mov_actual.AffectsAllTeam()){
			if(target_disposition == 0){
				for(int i = enemyList.Count; i < allFightersList.Count; i++){
					if(!allFightersList[i].IsDead())
						alreadySelected[i] = true;
				}
			}else if(target_disposition == 1){
				for(int i = 0; i < enemyList.Count; i++){
					if(!allFightersList[i].IsDead())
						alreadySelected[i] = true;
				}
			}else{
				for(int i = 0; i < allFightersList.Count; i++){
					if(!allFightersList[i].IsDead())
						alreadySelected[i] = true;
				}
			}
		}
		else{
			alreadySelected[currentTargetIndex] = true;
		}
		num_selec--;
		TTS.PutThisInQueue($"{allFightersList[currentTargetIndex].GetEntityData().Name} seleccionado.");
		if(num_selec == 0 || mov_actual.AffectsAllTeam()){
			Fighter[] targets;
			int t = 0, q = 0, s = 0, v = 0;
			foreach (bool aySd in alreadySelected){
				if(aySd){
					allFightersList[v].StopBlink();
					t++;
				}
				v++;
			}
			targets = new Fighter[t];
			foreach (bool aySd in alreadySelected){
				if(aySd){
					targets[q] = allFightersList[s];
					q++;
				}
				s++;
			}
			selectingTarget = false;
			ChangeMenu(-1);
			customSignals.EmitSignal(nameof(CustomSignals.OnTargetsConfirmed),actor,targets,mov_actual);
		}
		else{
			this.ChangeTarget(1);
		}
	}
	
	public void ShowMoveInfo(){
		Vector2 aux = new Vector2(0,0);
		if(currentButtonFocus == attack){
			aux = new Vector2(100,75);
			TTS.PutThisInQueue($"Info de ataque básico de {actor.GetEntityData().Name}");
			PrepareInfo(atqbas, false, false, true, aux);
		}else if(currentButtonFocus == special){
			string titulo = "Especiales";
			string descripcion = "Abre el menú de los movimientos especiales.";
			TTS.PutThisInQueue($"{titulo}. {descripcion}");
			labelInfoTitle.Text = $"{titulo}";
			labelInfoDescription.Text = $"{descripcion}";
			labelInfoPower.Text = "";
			labelInfoCost.Text = "";
			labelInfoLevelEvolve.Text = "";
			aux = new Vector2(50,75);
			panelInfo.Position = currentButtonFocus.Position + aux;
			panelInfo.UpdatePanelSize();
			panelInfo.Visible = true;
		}else if(currentButtonFocus == bag){
			string titulo = "Bolsa de objetos consumibles";
			string descripcion = "PROXIMAMENTE IMPLENTADA.\nAbre el menú de los objetos.";
			labelInfoTitle.Text = $"{titulo}";
			labelInfoDescription.Text = $"{descripcion}";
			labelInfoPower.Text = "";
			labelInfoCost.Text = "";
			labelInfoLevelEvolve.Text = "";
			aux = new Vector2(-50,75);
			panelInfo.Position = currentButtonFocus.Position + aux;
			panelInfo.UpdatePanelSize();
			panelInfo.Visible = true;
		}else if(currentButtonFocus == guard){
			aux = new Vector2(-200,75);
			TTS.PutThisInQueue($"Info de guardia de {actor.GetEntityData().Name}");
			PrepareInfo(guardia, false, false, true, aux);
		}else if(currentButtonFocus == mov1){
			aux = new Vector2(100,75);
			TTS.PutThisInQueue($"Info del movimiento especial de {actor.GetEntityData().Name}");
			PrepareInfo(sp1, true, true, true, aux);
		}else if(currentButtonFocus == mov2){
			aux = new Vector2(50,75);
			TTS.PutThisInQueue($"Info del movimiento especial de {actor.GetEntityData().Name}");
			PrepareInfo(sp2, true, true, true, aux);
		}else if(currentButtonFocus == mov3){
			aux = new Vector2(-50,75);
			TTS.PutThisInQueue($"Info del movimiento especial de {actor.GetEntityData().Name}");
			PrepareInfo(sp3, true, true, true, aux);
		}else if(currentButtonFocus == mov4){
			aux = new Vector2(-200,75);
			TTS.PutThisInQueue($"Info del movimiento especial de {actor.GetEntityData().Name}");
			PrepareInfo(sp4, true, true, true, aux);
		}
	}
	public void PrepareInfo(Skill s, bool c, bool l, bool e, Vector2  pos){
		string titulo = "";
		string descripcion = "";
		string cost = "";
		string potencia = "";
		string level = "";
		string evolve = "";
		string tts = "";
		titulo = s.GiveTitulo();
		descripcion = s.GiveDescription();
		if(c){
			cost = $"Coste: {cost}{s.ManaCost} Maná";
			tts += $"{cost}.";
		}
		if(s.Hurts){
			potencia = $"Potencia: {potencia}{s.Power} POW";
			tts += $"{s.Power} de potencia.";
		}
		if(!s.MoveIsAvailable()){
			level = $"NO DISPONIBLE - NIVEL {s.RequiredLevelToUnlock} REQUERIDO";
			tts += $"Nivel insuficiente. Se necesita nivel {s.RequiredLevelToUnlock} en este miembro.";
		}
		else if(!s.EnoughMana(actor)){
			level = $"NO DISPONIBLE - MANÁ INSUFICIENTE";
			tts += $"No se tiene suficiente maná para hacer este movmiento.";
		}
		else{
			level = $"LISTO Y CARGADO";
		}
		evolve = $"Nivel evolución: {evolve}{s.RequiredLevelToEvolve}";
		panelInfo.UpdatePanelSize();
		labelInfoTitle.Text = $"{titulo}";
		labelInfoDescription.Text = $"{descripcion}";
		tts += $"{titulo}. {descripcion}.";
		TTS.PutThisInQueue(tts);
		labelInfoPower.Text = $"{potencia}";
		labelInfoCost.Text = $"{cost}";
		labelInfoLevelEvolve.Text = $"{evolve}";
		
		panelInfo.Position = currentButtonFocus.Position + pos;
		panelInfo.UpdatePanelSize();
		panelInfo.Visible = true;
	}
	
}
