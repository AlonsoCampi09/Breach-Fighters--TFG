using Godot;
using System;

public partial class Game : Node2D{
	private FighterTeam teamJugador;
	private FighterTeam teamEnemy;
	
	private GameState gameState;
	private UiMessages uiMessages;
	private CustomSignals customSignals;
	private TransitionScreen transitionScreen;
	private BattleManager battleManager;
	private RocksRest restRocks;
	private CuadroTexto cuadro;
	
	private PackedScene fighterTeamScene;
	private AudioStreamPlayer2D music;
	
	private bool combatTime = false;
	private bool restGeneratedRandom = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		fighterTeamScene = ResourceLoader.Load<PackedScene>("res://global/scenes/team.tscn");
		teamJugador = GetNode<FighterTeam>("TeamPlayer");
		gameState = GetNode<GameState>("/root/GameState");
		uiMessages = GetNode<UiMessages>("/root/UiMessages");
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		transitionScreen = GetNode<TransitionScreen>("/root/TransitionScreen");
		battleManager = GetNode<BattleManager>("/root/BattleManager");
		optionsMenu = GetNode<MenuOpcionesGlobal>("UI/Menu_Opciones");
		Menu_Batalla = GetNode<MenuBatalla>("UI/Menu_Batalla");
		uiMessages.GiveLabel(GetNode<Label>("UI/RoomLabel"));
		cuadro = GetNode<CuadroTexto>("UI/Cuadro_Texto");
		music = GetNode<AudioStreamPlayer2D>("Music");
		restRocks = GetNode<RocksRest>("Rest");
		
		customSignals.OnPrepareRoom += FightOrRest;
		customSignals.OnGenerateEnemyTeamForRoom += GenerateBattleOponents;
		customSignals.OnStartTeamBattle += PrepareBattle;
		customSignals.OnNextRoom += GoNextLevel;
		customSignals.OnRestFinished += GoingNextLevel;
		music.Finished += PlayMusicAgain;
		
		restRocks.Visible = false;
		
		GeneratePlayerTeam();
		GenerateTypeRoom();
		uiMessages.ShowRoomInfo(gameState);
		// Generar el contenido de la sala (combate o descanso)
	}
	
	public void PlayMusicAgain(){
		music.Play();
	}
	
	public void GeneratePlayerTeam(){
		Entity[] fighterDatas = new Entity[] {
			ResourceLoader.Load<Entity>("res://data/entities/Alex.tres"),
			ResourceLoader.Load<Entity>("res://data/entities/Cassandra.tres"),
			ResourceLoader.Load<Entity>("res://data/entities/Vyls.tres"),
			ResourceLoader.Load<Entity>("res://data/entities/Ishimondo.tres")
		};
		teamJugador.CreateTeam(fighterDatas,true);
	}
	
	public void GenerateTypeRoom(){
		if(gameState.floorRoom == 0){
			combatTime = false;
			restGeneratedRandom = false;
			restRocks.Visible = true;
			teamJugador.Visible = false;
		}else if(gameState.floorRoom == 9){
			combatTime = false;
			restRocks.Visible = false;
			restRocks.Visible = true;
			teamJugador.Visible = false;
		}
		else{
			if (GD.Randf() < 0.85f && !restGeneratedRandom) { //85% probabilidad de combate
				combatTime = true;
				restRocks.Visible = false;
				teamJugador.Visible = true;
			}
			else{
				restGeneratedRandom = true;
				combatTime = false;
				restRocks.Visible = true;
				teamJugador.Visible = false;
			}
		}
	}
	public void FightOrRest(){
		if(combatTime){
			uiMessages.ShowDangerAlert();
		}else{
			teamJugador.Rest();
			customSignals.EmitSignal(nameof(CustomSignals.OnRestRecharge), gameState, teamJugador, restRocks);
			uiMessages.RestingMessage();
		}
	}
	
	public async void GoNextLevel(int teamExp, int teamCoins){
		teamJugador.Revive();
		gameState.AdavanceNextRoom(teamExp,teamCoins);
		await transitionScreen.fade_to_black();
		GenerateTypeRoom();
		await transitionScreen.fade_to_normal();
		uiMessages.ShowRoomInfo(gameState);
	}
	public async void GoingNextLevel(){
		gameState.AdavanceNextRoom(0,0);
		await transitionScreen.fade_to_black();
		GenerateTypeRoom();
		await transitionScreen.fade_to_normal();
		uiMessages.ShowRoomInfo(gameState);
	}
	
	public void GenerateBattleOponents(){
		GD.Print("Cargando escena FighterTeam...");
		GD.Print("Buscando FighterFactory...");
		FighterFactory fighterFactory = GetNode<FighterFactory>("/root/FighterFactory");
		teamEnemy = fighterTeamScene.Instantiate<FighterTeam>();
		AddChild(teamEnemy);
		Vector2 centerScreen = GetViewport().GetVisibleRect().Size / 2;
		centerScreen.Y -= 75;
		teamEnemy.Position = centerScreen;
		Entity[] datas = fighterFactory.GenerateRandomEntityDatas(gameState.playerLevel);
		teamEnemy.CreateTeam(datas, false);
		TTS.PutThisInQueue($"El equipo se enfrenta a {datas.Length} enemigos");
		uiMessages.ReadyForTheBattle();
		
	}
	
	public void PrepareBattle(){
		battleManager.StartBattle(teamJugador, teamEnemy);
	}
}
