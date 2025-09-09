using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FighterTeam : Node2D
{
	private int teamSize;
	private bool controlJugador;
	private FighterFactory fighterFactory;
	private List<Fighter> fighterList = new List<Fighter>();
	

	public override void _Ready(){
		fighterFactory = GetNode<FighterFactory>("/root/FighterFactory");
		/*
		Entity[] fighterDatas = new Entity[] {
			ResourceLoader.Load<Entity>("res://data/puntotres/Alex.tres"),
			ResourceLoader.Load<Entity>("res://data/puntotres/Cassandra.tres"),
			ResourceLoader.Load<Entity>("res://data/puntotres/Vyls.tres"),
			ResourceLoader.Load<Entity>("res://data/puntotres/Ishimondo.tres")
		};
		*/
		//Entity fighterData = ResourceLoader.Load<Entity>("res://data/puntotres/Alex.tres");
		/*
		Fighter aux = fighterFactory.CreateFighter(fighterData);

		AddChild(aux);
		AddChild(teamContainer);
		aux.Position = new Vector2(0, 0); // Por ejemplo
		*/
	}
	
	public void CreateTeam(Entity[] fighterDatas, bool control){
		teamSize = fighterDatas.Length;
		controlJugador = control;
		int spacing = 0;
		for(int i = 0; i < fighterDatas.Length; i++){
			spacing += fighterDatas[i].FrameWidth;
		}
		int averageSpacing = spacing / fighterDatas.Length;
		float totalWidth = (teamSize - 1) * averageSpacing;
		float startX = -totalWidth / 2; // Punto inicial para centrar
		for (int i = 0; i < teamSize; i++){
			Fighter aux = fighterFactory.CreateFighter(fighterDatas[i]);
			AddChild(aux);
			fighterList.Add(aux);
			spacing = fighterDatas[i].FrameWidth; 
			float x = startX + i * averageSpacing;
			float y = 0; // Puedes variar Y si quieres otros efectos
			aux.Position = new Vector2(x, y);
		}
	}
	
	public void LoadFromEntities(List<Entity> f, bool control){
		ClearTeam();
		CreateTeam(f.ToArray(), control);
	}
	
	public void ClearTeam(){
		foreach(var fighter in fighterList){
			fighter.QueueFree();
		}
		fighterList.Clear();
	}
	public int AllFightersAvailable(){
		int i = 0, j = 0;
		while(i < fighterList.Count){
			if(!fighterList[i].IsDead())	j++;
			i++;
		}
		return j;
	}
	public bool AllFightersDead(){
		bool allDead = true;
		int i = 0;
		while(i < fighterList.Count && allDead){
			allDead = fighterList[i].IsDead();
			i++;
		}
		return allDead;
	}
	
	public void Revive(){
		bool revived = false;
		for(int i = 0; i < fighterList.Count; i++){
			if(fighterList[i].IsDead()){
				fighterList[i].Revive();
				revived = true;
			}
		}
		if(revived)
			TTS.SayThis("Miembros abatidos reanimados.");
	}
	public void ResetStatus(){
		for(int i = 0; i < fighterList.Count; i++){
				fighterList[i].Rst();
		}
	}
	public void Rest(){
		for(int i = 0; i < fighterList.Count; i++){
			fighterList[i].FullMana();
			fighterList[i].FullHealth();
		}
		TTS.SayThis("Vida y maná restaurada");
	}
	
	public List<Fighter> GetFighters(){
		return fighterList;
	}
	
	public void FreeTeam(){
		for(int i = 0; i < fighterList.Count; i++){
			fighterList[i].FreeFighter();
		}
		fighterList.Clear();
		QueueFree();
	}
	
	
}
