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
	private MenuOpcionesGlobal optionsMenu; 
	
	private PackedScene fighterTeamScene;
	
	
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
		uiMessages.GiveLabel(GetNode<Label>("UI/RoomLabel"));

		optionsMenu.Hide(); 
		customSignals.OnPrepareRoom += GenerateRoom;
		customSignals.OnGenerateEnemyTeamForRoom += GenerateBattleOponents;
		customSignals.OnStartTeamBattle += PrepareBattle;
		customSignals.OnNextRoom += GoNextLevel;
		
		GeneratePlayerTeam();
		uiMessages.ShowRoomInfo(gameState);
		// Generar el contenido de la sala (combate o descanso)
	}


    public override void _Process(double delta)
    {
		if (Input.IsActionJustPressed("pause_menu"))
		{
			OpenPauseMenu(); 
		} 

    }


	public void OpenPauseMenu()
	{

		if (optionsMenu.Visible) {
            optionsMenu.Hide();
            GetTree().Paused = false;
        }

		else
		{
			GetTree().Paused = true;
            optionsMenu.Show();
        }

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

	public void GenerateRoom(){
		/*
		if(gameState.floorRoom == 1 || gameState.floorRoom == 9){
			//StartRest();
		}
		else{
			if (GD.Randf() < 1.0f) { //80% probabilidad de combate
				GenerateBattleOponents();
			}
			else{
				//StartRest();
			}
		}
		*/
		if (GD.Randf() < 1.0f) { //80% probabilidad de combate
			uiMessages.ShowDangerAlert();
		}
		else{
			//StartRest();
		}
	}
	
	public async void GoNextLevel(int teamExp, int teamCoins){
		gameState.AdavanceNextRoom(teamExp,teamCoins);
		GD.Print("HOLA1");
		await transitionScreen.fade_to_black();
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
