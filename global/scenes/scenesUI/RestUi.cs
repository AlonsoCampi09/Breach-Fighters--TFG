using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RestUi : Control{
	
	private int estado = 0;
	private CustomSignals customSignals;
	private VBoxContainer restOptions;
	private Button teamSelect;
	private Button restExit;
	
	private Panel characterInfo;
	private Label actor_name;
	private TextureRect actor_sprite;
	private Label actor_level;
	private Label actor_health;
	private Label actor_attack;
	private Label actor_defense;
	private Label actor_mana;
	private Label actor_speed;
	private Label actor_experience;
	private Label actor_level_up;
	private Label actor_health_up;
	private Label actor_attack_up;
	private Label actor_defense_up;
	private Label actor_mana_up;
	private Label actor_speed_up;
	private Label actor_experience_up;
	
	private Panel moneyExp;
	private Label money;
	private Label exp;
	
	private Panel characterOptions;
	private Button levelUp;
	private Button skills;
	private Button back;
	
	private Panel characterSkills;
	private Button atq;
	private Button guard;
	private Button mov1;
	private Button mov2;
	private Button mov3;
	private Button mov4;
	private Button back2;
	private InfoPanel panelInfo;
	private Label labelInfoTitle;
	private Label labelInfoDescription;
	private Label labelInfoPower;
	private Label labelInfoCost;
	private Label labelInfoLevelNeeded;
	private Label labelInfoLevelEvolve;
	
	private Panel levelUpPanel;
	
	private Panel panelPopUp;
	
	private Panel selecting;
	private Button inv;
	
	private bool selectingTarget;
	private int indexTargetStart = 0;
	private int indexTargetEnd = 0;
	private int currentTargetIndex = 0; // Índice del enemigo seleccionado
	
	private GameState gameStatus;
	private List<Fighter> allyList;
	private RocksRest alliesRest;
	private Fighter actor;
	private Skill atqbas;
	private Skill guardia;
	private Skill sp1;
	private Skill sp2;
	private Skill sp3;
	private Skill sp4;
	
	private Button currentButtonFocus;
	
	bool acceptingInputs = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		
		restOptions = GetNode<VBoxContainer>("RestOptions");
		teamSelect = GetNode<Button>("RestOptions/Team");
		restExit = GetNode<Button>("RestOptions/Exit");
		
		teamSelect.Pressed += OnTeamButtonPressed;
		restExit.Pressed += OnExitButtonPressed;
		teamSelect.FocusEntered += OnTeamButtonFocused;
		restExit.FocusEntered += OnExitButtonFocused;
		
		moneyExp = GetNode<Panel>("MoneyExp");
		money = GetNode<Label>("MoneyExp/MarginContainer/Container/Money/Label");
		exp = GetNode<Label>("MoneyExp/MarginContainer/Container/Experience/Label");
		
		characterInfo = GetNode<Panel>("CharacterInfo");
		actor_name = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/Name");
		actor_sprite = GetNode<TextureRect>("CharacterInfo/MarginContainer/VBoxContainer/Sprite");
		actor_level = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Level/Label");
		actor_health = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Health/Label");
		actor_attack = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Attack/Label");
		actor_defense = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Defense/Label");
		actor_mana = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Mana/Label");
		actor_speed = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Speed/Label");
		actor_experience = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Experience/Label");
		actor_level_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Level/Label3");
		actor_health_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Health/Label3");
		actor_attack_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Attack/Label3");
		actor_defense_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Defense/Label3");
		actor_mana_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Mana/Label3");
		actor_speed_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Speed/Label3");
		actor_experience_up = GetNode<Label>("CharacterInfo/MarginContainer/VBoxContainer/StatsInfo/Experience/Label3");
		
		characterOptions = GetNode<Panel>("CharacterOptions");
		levelUp = GetNode<Button>("CharacterOptions/MarginContainer/VBoxContainer/LevelUp");
		skills = GetNode<Button>("CharacterOptions/MarginContainer/VBoxContainer/Skills");
		back = GetNode<Button>("CharacterOptions/MarginContainer/VBoxContainer/Back");
		
		levelUp.Pressed += OnLevelButtonPressed;
		skills.Pressed += OnSkillButtonPressed;
		back.Pressed += OnBack1ButtonPressed;
		levelUp.FocusEntered += OnLevelButtonFocused;
		skills.FocusEntered += OnSkillButtonFocused;
		back.FocusEntered += OnBackButtonFocused;
		
		characterSkills = GetNode<Panel>("CharacterSkills");
		atq = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Skill1");
		guard = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Skill2");
		mov1 = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Skill3");
		mov2 = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Skill4");
		mov3 = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Skill5");
		mov4 = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Skill6");
		back2 = GetNode<Button>("CharacterSkills/MarginContainer/ScrollContainer/VBoxContainer/Back");
		panelInfo = GetNode<InfoPanel>("Info_Panel");
		labelInfoTitle = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/Title");
		labelInfoDescription = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/Description");
		labelInfoPower = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/Power");
		labelInfoCost = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer2/Cost");
		labelInfoLevelNeeded = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/Level_Needed");
		labelInfoLevelEvolve = GetNode<Label>("Info_Panel/MarginContainer/VBoxContainer/HBoxContainer/VBoxContainer/Level_Evolve");
		
		atq.FocusEntered += OnAttackFocusEntered;
		guard.FocusEntered += OnGuardFocusEntered;
		mov1.FocusEntered += OnMov1FocusEntered;
		mov2.FocusEntered += OnMov2FocusEntered;
		mov3.FocusEntered += OnMov3FocusEntered;
		mov4.FocusEntered += OnMov4FocusEntered;
		back2.FocusEntered += OnBack2FocusEntered;
		back2.Pressed += OnBack2ButtonPressed;
		
		selecting = GetNode<Panel>("Selection");
		inv = GetNode<Button>("Selection/MarginContainer/HBoxContainer/Inv");
		
		inv.Pressed += OnInvButtonPressed;
		
		levelUpPanel = GetNode<Panel>("LevelUpPanel");
		
		panelPopUp = GetNode<Panel>("PanelPopUp");
		
		customSignals.OnRestRecharge += RechargingGameTeamState;
		customSignals.OnShowRestHUD += MakeMenuVisible;
		
		restOptions.Visible = false;
		moneyExp.Visible = false;
		characterInfo.Visible = false;
		characterSkills.Visible = false;
		characterOptions.Visible = false;
		panelInfo.Visible = false;
		selecting.Visible = false;
		levelUpPanel.Visible = false;
		panelPopUp.Visible = false;
	}
	
	public void RechargingGameTeamState(GameState gs, FighterTeam allies, RocksRest rest){
		gameStatus = gs;
		allyList = allies.GetFighters();
		alliesRest = rest;
		ShowMoneyExp();
	}
	
	public void ShowMoneyExp(){
		money.Text = $"{gameStatus.teamMoneyBank}";
		exp.Text = $"{gameStatus.teamExperienceBank}";
	}
	
	public void MakeMenuVisible(){
		GD.Print("MakingMenuVisible...");
		acceptingInputs = true;
		this.ChangeMenu(0);
	}
	
	private void ChangeMenu(int c){
		switch (c)
		{
			case -1:
				estado = -1;
				restOptions.Visible = false;
				moneyExp.Visible = false;
				characterInfo.Visible = false;
				characterSkills.Visible = false;
				characterOptions.Visible = false;
				panelInfo.Visible = false;
				selecting.Visible = false;
				levelUpPanel.Visible = false;
				panelPopUp.Visible = false;
				acceptingInputs = false;
				this.selectingTarget = false;
				for(int i = 0; i < allyList.Count; i++){
					alliesRest.StopBlink(i);
				}
				break;
			case 0:
				estado = 0;
				restOptions.Visible = true;
				moneyExp.Visible = true;
				characterInfo.Visible = false;
				characterSkills.Visible = false;
				characterOptions.Visible = false;
				panelInfo.Visible = false;
				selecting.Visible = false;
				levelUpPanel.Visible = false;
				panelPopUp.Visible = false;
				this.selectingTarget = false;
				for(int i = 0; i < allyList.Count; i++){
					alliesRest.StopBlink(i);
				}
				teamSelect.GrabFocus();
				break;
			case 1:
				estado = 1;
				restOptions.Visible = false;
				moneyExp.Visible = true;
				characterInfo.Visible = false;
				characterSkills.Visible = false;
				characterOptions.Visible = false;
				panelInfo.Visible = false;
				selecting.Visible = true;
				levelUpPanel.Visible = false;
				panelPopUp.Visible = false;
				selecting.Modulate = new Color(1, 1, 1, 0); // Hacerlo invisible
				inv.GrabFocus();
				this.StartTargetSelection();
				break;
			case 2:
				estado = 2;
				restOptions.Visible = false;
				moneyExp.Visible = true;
				characterInfo.Visible = true;
				characterSkills.Visible = false;
				characterOptions.Visible = true;
				panelInfo.Visible = false;
				selecting.Visible = false;
				levelUpPanel.Visible = false;
				panelPopUp.Visible = false;
				EvolveStats(false);
				levelUp.GrabFocus();
				break;
			case 3:
				estado = 3;
				restOptions.Visible = false;
				moneyExp.Visible = true;
				characterInfo.Visible = true;
				characterSkills.Visible = true;
				characterOptions.Visible = false;
				panelInfo.Visible = false;
				selecting.Visible = false;
				levelUpPanel.Visible = false;
				panelPopUp.Visible = false;
				atq.GrabFocus();
				break;
			case 4:
				estado = 4;
				restOptions.Visible = false;
				moneyExp.Visible = true;
				characterInfo.Visible = true;
				characterSkills.Visible = false;
				characterOptions.Visible = true;
				panelInfo.Visible = false;
				selecting.Visible = true;
				levelUpPanel.Visible = true;
				panelPopUp.Visible = false;
				selecting.Modulate = new Color(1, 1, 1, 0); // Hacerlo invisible
				inv.GrabFocus();
				EvolveStats(true);
				TTS.SayThis($"Poses {gameStatus.teamExperienceBank} puntos de experiencia. Para subir de nivel son necesarios {actor.GetEntityData().giveEXPsigLevel()}. El personaje tendrá: {actor_health_up.Text}, {actor_attack_up.Text}, {actor_defense_up.Text}, {actor_mana_up.Text}, {actor_speed_up.Text}. Para confirmar pulsa espacio. Para cancelar pulse X.");
				break;
			case 5:
				estado = 5;
				restOptions.Visible = false;
				moneyExp.Visible = false;
				characterInfo.Visible = false;
				characterSkills.Visible = false;
				characterOptions.Visible = false;
				panelInfo.Visible = false;
				selecting.Visible = true;
				levelUpPanel.Visible = false;
				panelPopUp.Visible = true;
				selecting.Modulate = new Color(1, 1, 1, 0); // Hacerlo invisible
				TTS.SayThis($"Quieres salir del descanso? Para confirmar pulsa espacio. Para cancelar pulse X.");
				inv.GrabFocus();
				break;
		}
	}
	
	private void EvolveStats(bool a){
		actor_level_up.Visible = a;
		actor_health_up.Visible = a;
		actor_attack_up.Visible = a;
		actor_defense_up.Visible = a;
		actor_mana_up.Visible = a;
		actor_speed_up.Visible = a;
		actor_experience_up.Visible = a;
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if (Input.IsActionJustPressed("ui_cancel") && acceptingInputs){
			switch(estado){
				case 1: case 2: case 5:
					ChangeMenu(0);
					break;
				case 3: case 4:
					ChangeMenu(2);
					break;
			}
		}
		if(Input.IsActionJustPressed("custom_ui_turner_info") && estado == 2 && acceptingInputs){
			TTS.SayThis($"{actor_name.Text}, {actor_level.Text}, {actor_health.Text}, {actor_attack.Text}, {actor_defense.Text}, {actor_mana.Text}, {actor_speed.Text}");
		}
		if(Input.IsActionJustPressed("custom_ui_fighter_info") && acceptingInputs){
			GD.Print("HOLA");
			TTS.SayThis($"En la sala aparece el equipo descansando. Alex está sentado grabateando en su cuaderno. Cassandra está meditando. Vyls está sentado cerca de Cassandra. Ishimondo acostado en el suelo combatiendo las ganas de dormirse.");
		}
		if (this.selectingTarget){
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
	
	private void OnTeamButtonPressed(){
		ChangeMenu(1);
	}
	private void OnExitButtonPressed(){
		ChangeMenu(5);
	}
	private void OnInvButtonPressed(){
		switch(estado){
			case 1:
				GD.Print("Target CONFIRM");
				this.ConfirmTarget();
				break;
			case 4:
				GD.Print("CONFIRM");
				gameStatus.UsedExp(actor.GetEntityData().giveEXPsigLevel());
				actor.LevelUp();
				PrepareCharacterInfoPanelAndSkills();
				ChangeMenu(2);
				ShowMoneyExp();
				break;
			case 5:
				GD.Print("CONFIRM");
				customSignals.EmitSignal(nameof(CustomSignals.OnRestFinished));
				ChangeMenu(-1);
				
				break;
		}
	}
	private void OnLevelButtonPressed(){
		GD.Print("Aqui");
		ChangeMenu(4);
	}
	private void OnSkillButtonPressed(){
		ChangeMenu(3);
	}
	private void OnBack1ButtonPressed(){
		ChangeMenu(0);
	}
	private void OnBack2ButtonPressed(){
		ChangeMenu(2);
	}
	
	private void ReloadStats(){
		actor_name.Text = actor.GetEntityData().Name;
		actor_sprite.Texture = actor.GiveTexture();
		actor_level.Text = $"Nivel: {actor.GetEntityData().Level}";
		actor_health.Text = $"Vida: {actor.GetEntityData().giveMAXHP()}";
		actor_attack.Text = $"Ataque: {actor.GetEntityData().giveDMG()}";
		actor_defense.Text = $"Defensa: {actor.GetEntityData().giveDEF()}";
		actor_mana.Text = $"Maná: {actor.GetEntityData().giveMAXMP()}";
		actor_speed.Text = $"Velocidad: {actor.GetEntityData().giveSP()}";
		actor_experience.Text = $"Experiencia: {actor.GetEntityData().giveEXPsigLevel()}";
		actor_level_up.Text = $"Nivel: {actor.GetEntityData().Level+1}";
		actor_health_up.Text = $"Vida: {actor.GetEntityData().giveMAXHPUP()}";
		actor_attack_up.Text = $"Ataque: {actor.GetEntityData().giveDMGUP()}";
		actor_defense_up.Text = $"Defensa: {actor.GetEntityData().giveDEFUP()}";
		actor_mana_up.Text = $"Maná: {actor.GetEntityData().giveMAXMPUP()}";
		actor_speed_up.Text = $"Velocidad: {actor.GetEntityData().giveSPUP()}";
		actor_experience_up.Text = $"Experiencia: {actor.GetEntityData().giveEXPsigLevelUP()}";
		if(gameStatus.teamExperienceBank < actor.GetEntityData().giveEXPsigLevel()){
			levelUp.Disabled = true;
		}
	}
	
	private void PrepareCharacterInfoPanelAndSkills(){
		actor = allyList[currentTargetIndex];
		atqbas = actor.AtqBasico();
		guardia = actor.Guardia();
		sp1 = actor.Skill1();
		sp2 = actor.Skill2();
		sp3 = actor.Skill3();
		sp4 = actor.Skill4();
		PrepareButtons(mov1,sp1);
		PrepareButtons(mov2,sp2);
		PrepareButtons(mov3,sp3);
		PrepareButtons(mov4,sp4);
		if(atqbas != null){
			atq.Text = atqbas.GiveTitulo();
		}else{
			atq.Text = "NULL";
			atq.Visible = false;
		}
		if(guardia != null){
			guard.Text = guardia.GiveTitulo();
		}else{
			guard.Text = "NULL";
			guard.Visible = false;
		}
		ReloadStats();
	}
	public void PrepareButtons(Button b, Skill s){
		string t = "";
		bool d = true;
		if(s != null){
			if(s.MoveIsAvailable()){
				t = s.DisplayName;
				
			}else{
				t = $"NIVEL  {s.RequiredLevelToUnlock}";
				d = false;
			}
		}else{
			t = "NULL";
			d = false;
		}
		b.Text = t;
		b.Visible = d;
	}
	
	private void StartTargetSelection(){
		this.selectingTarget = true;
		indexTargetStart = 0;
		indexTargetEnd = allyList.Count;
		currentTargetIndex = indexTargetStart;
		this.ChangeTarget(0);
	}
	private void AimToCurrentTarget(){
		alliesRest.StartBlink(currentTargetIndex);
	}
	private void ChangeTarget(int direction){
		alliesRest.StopBlink(currentTargetIndex);
		currentTargetIndex = currentTargetIndex + direction;
		if(currentTargetIndex <= -1){
			currentTargetIndex = indexTargetEnd-1;
		}else if(currentTargetIndex >= indexTargetEnd){
			currentTargetIndex = indexTargetStart;
		}
		TTS.SayThis($"{allyList[currentTargetIndex].GetEntityData().Name}");
		AimToCurrentTarget();
	}
	private void ConfirmTarget(){
		this.selectingTarget = false;
		for(int i = 0; i < allyList.Count; i++){
			alliesRest.StopBlink(i);
		}
		TTS.PutThisInQueue($"{allyList[currentTargetIndex].GetEntityData().Name} seleccionado.");
		PrepareCharacterInfoPanelAndSkills();
		ChangeMenu(2);
	}
	
	private void OnTeamButtonFocused(){
		TTS.SayThis($"Equipo. Una vez selecciones al miembro que quieres ver puedes pulsar F2 para oir sus estadísticas.");
	}
	private void OnExitButtonFocused(){
		TTS.SayThis($"Salir del descanso");
	}
	private void OnLevelButtonFocused(){
		if(levelUp.Disabled == true){
			TTS.SayThis($"Subir de nivel. Nivel Insuficiente");
		}else{
			TTS.SayThis($"Subir de nivel. Disponible");
		}
	}
	private void OnSkillButtonFocused(){
		TTS.SayThis($"Ver habilidades");
	}
	private void OnBack2FocusEntered(){
		TTS.SayThis($"Atrás");
		panelInfo.Visible = false;
	}
	private void OnAttackFocusEntered(){
		currentButtonFocus = atq;
		panelInfo.Visible = true;
		ShowMoveInfo();
	}
	private void OnGuardFocusEntered(){
		currentButtonFocus = guard;
		panelInfo.Visible = true;
		ShowMoveInfo();
	}
	private void OnMov1FocusEntered(){
		currentButtonFocus = mov1;
		panelInfo.Visible = true;
		ShowMoveInfo();
	}
	private void OnMov2FocusEntered(){
		currentButtonFocus = mov2;
		panelInfo.Visible = true;
		ShowMoveInfo();
	}
	private void OnMov3FocusEntered(){
		currentButtonFocus = mov3;
		panelInfo.Visible = true;
		ShowMoveInfo();
	}
	private void OnMov4FocusEntered(){
		currentButtonFocus = mov4;
		panelInfo.Visible = true;
		ShowMoveInfo();
	}
	private void OnBackButtonFocused(){
		TTS.SayThis("Atrás");
	}
	
	public void ShowMoveInfo(){
		Vector2 aux = new Vector2(0,0);
		if(currentButtonFocus == atq){
			TTS.SayThis($"Ataque básico");
			PrepareInfo(atqbas, false, false, true);
		}else if(currentButtonFocus == guard){
			TTS.SayThis($"Guardia");
			PrepareInfo(guardia, false, false, true);
		}else if(currentButtonFocus == mov1){
			TTS.SayThis("");
			PrepareInfo(sp1, true, true, true);
		}else if(currentButtonFocus == mov2){
			TTS.SayThis("");
			PrepareInfo(sp2, true, true, true);
		}else if(currentButtonFocus == mov3){
			TTS.SayThis("");
			PrepareInfo(sp3, true, true, true);
		}else if(currentButtonFocus == mov4){
			TTS.SayThis("");
			PrepareInfo(sp4, true, true, true);
		}
	}
	public void PrepareInfo(Skill s, bool c, bool l, bool e){
		string titulo = "";
		string descripcion = "";
		string cost = "";
		string potencia = "";
		string level = "";
		string evolve = "";
		string tts = "";
		titulo = s.GiveTitulo();
		descripcion = s.GiveDescription();
		tts += $"{titulo}. {descripcion}.";
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
		labelInfoPower.Text = $"{potencia}";
		labelInfoCost.Text = $"{cost}";
		labelInfoLevelEvolve.Text = $"{evolve}";
		TTS.PutThisInQueue(tts);
		
		panelInfo.UpdatePanelSize();
		panelInfo.Visible = true;
	}
}
