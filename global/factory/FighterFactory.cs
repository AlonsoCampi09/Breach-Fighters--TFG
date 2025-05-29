using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FighterFactory : Node
{
	private PackedScene FighterScene;
	
	public override void _Ready(){
		FighterScene = ResourceLoader.Load<PackedScene>("res://global/scenes/fighter.tscn");
	}

	public Fighter CreateFighter(Entity entityData){
		List<Skill> skills;
		if (FighterScene == null){
			FighterScene = ResourceLoader.Load<PackedScene>("res://global/scenes/fighter.tscn"); //Segundo intento.
			if (FighterScene == null){
				GD.PrintErr("Error: FighterScene no cargado. Revisar FighterFactory.CreateFighter(Entity entityData) o todas las llamadas anteriores.");
				return null;
			}
		}
		skills = PrepareFighterSkills(entityData);
		Fighter newFighter = FighterScene.Instantiate<Fighter>();
		newFighter.SetEntityData(entityData);
		newFighter.SetSkills(skills);
		newFighter.AsignLevel(entityData.Level);
		return newFighter;
	}
	
	public List<Skill> PrepareFighterSkills(Entity entityData){
		List<Skill> skills = new List<Skill>();
		string name = entityData.Name;
		if(name.Contains("Alex") || name.Contains("Chuvakan")){
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/AlexBasicSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/AlexGuardSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/AlexSpecial1.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/AlexSpecial2.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/AlexSpecial3.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/AlexSpecial4.tres"));
		}else if(name.Contains("Cassandra")){
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/CassBasicSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/CassGuardSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/CassSpecial1.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/CassSpecial2.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/CassSpecial3.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/CassSpecial4.tres"));
		}else if(name.Contains("Ishimondo")){
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/IshiBasicSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/IshiGuardSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/IshiSpecial1.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/IshiSpecial2.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/IshiSpecial3.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/IshiSpecial4.tres"));
		}else if(name.Contains("Vyls")){
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/VylsBasicSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/VylsGuardSkill.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/VylsSpecial1.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/VylsSpecial2.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/VylsSpecial3.tres"));
			skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/VylsSpecial4.tres"));
		}else if(name.Contains("Slime")){
			if(name.Contains("Agresivo")){
				skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/SAgresivoBasicSkill.tres"));
			}else if(name.Contains("Inocente")){
				skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/SInocenteBasicSkill.tres"));
			}else if(name.Contains("Triste")){
				skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/STristeBasicSkill.tres"));
			}else if(name.Contains("Vago")){
				skills.Add(ResourceLoader.Load<Skill>("res://data/movimientos/SVagoBasicSkill.tres"));
			}else{
				GD.Print("MAL MAL. SLIME MAL.");
			}
		}else{
			GD.Print("No deberia entrar aqui.");
		}
		return skills;
	}
	
	public Entity[] GenerateRandomEntityDatas(int level){
		Random rand = new Random();
		List<Entity> list = new List<Entity>();
		int num_enemigos = Math.Min(6,rand.Next(2, 5+level)) - 1; //En un equipo enemigo habra de 1 a 5 entities
		for(int i = 0; i < num_enemigos; i++){
			int enemigo = rand.Next(0, 4); // De momento solo hay 4 enemigos subditos
			int l = rand.Next(1, 2+level);
			switch(enemigo){
				case 0:
					list.Add((Entity)ResourceLoader.Load<Entity>("res://data/entities/SlimeInocente.tres").Duplicate());
					break;
				case 1:
					list.Add((Entity)ResourceLoader.Load<Entity>("res://data/entities/SlimeAgresivo.tres").Duplicate());
					break;
				case 2:
					list.Add((Entity)ResourceLoader.Load<Entity>("res://data/entities/SlimeVago.tres").Duplicate());
					break;
				case 3:
					list.Add((Entity)ResourceLoader.Load<Entity>("res://data/entities/SlimeTriste.tres").Duplicate());
					break;
				default:
					break;
			}
			list[i].AssignLevel(l);
		}
		return list.ToArray();
	}
	
}
