using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameState : Node{
	public List<Entity> playerTeamData = new List<Entity>();
	public int teamExperienceBank = 5;
	public int teamMoneyBank = 100;
	public int playerLevel = 1;
	public int[] playerLevelExp = new int[] {12, 20, 30, 42, 54, 9999};
	public int actualPlayerExp = 0;
	
	
	public int floorLevel = 0;
	public int  floorRoom = 0;
	
	public void SaveTeamData(FighterTeam team){
		playerTeamData.Clear();
		foreach(Fighter f in team.GetFighters()){
			playerTeamData.Add(f.GetEntityData());
		}
	}
	
	public void UsedExp(int exp){
		teamExperienceBank -= exp;
	}
	public void UsedMoney(int mon){
		teamMoneyBank -= mon;
	}
	
	public void LoadTeamData(FighterTeam team){
		team.LoadFromEntities(playerTeamData, true);
	}
	
	public void AdavanceNextRoom(int exp, int coins){
		actualPlayerExp += 2;
		if(actualPlayerExp>=playerLevelExp[playerLevel]){
			actualPlayerExp -= playerLevelExp[playerLevel];
			playerLevel++;
		}
		teamMoneyBank += coins;
		teamExperienceBank += exp;
		floorRoom++;
		if(floorRoom > 10){
			floorLevel++;
			floorRoom = 0;
		}
		GD.Print($"coins: {teamMoneyBank} | Experiencia: {teamExperienceBank}");
	}
}
