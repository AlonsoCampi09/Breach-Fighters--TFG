using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AICombatManager : Node{
	
	private Fighter owner;
	private CustomSignals customSignals;
	
	private List<Fighter> fighterLastHitMe;
	
	private List<Skill> skills;
	private int[] Cooldowns;
	
	public override void _Ready(){
		owner = GetParent() as Fighter;
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		fighterLastHitMe = new List<Fighter>();
	}
	
	public void AIRandomDecision(FighterTeam hisTeam, FighterTeam vsTeam){
		Skill s = PickRandomSkillAvaible();
		Fighter[] selectedTargets = {null};
		switch(s.WhoAffects()){
			case 0:
				GD.Print("Affects his team");
				selectedTargets = PickRandomFighters(s,hisTeam);
				break;
			case 1:
				GD.Print("Affects his enemies");
				selectedTargets = PickRandomFighters(s,vsTeam);
				break;
			case 2:
				GD.Print("Can affect either both teams");
				Random rand = new Random();
				int i = rand.Next(0,2);
				if(i == 0)	selectedTargets = PickRandomFighters(s,hisTeam);
				else	selectedTargets = PickRandomFighters(s,vsTeam);
				break;
			case 3:
				GD.Print("Affects his team");
				if(s.AffectsAllTeam())
					selectedTargets = PickAllFightersAvaible(hisTeam.AllFightersAvailable(), hisTeam.GetFighters());
				else{
					selectedTargets = new Fighter[1];
					selectedTargets[0] = owner;
				}
				selectedTargets = PickRandomFighters(s,hisTeam);
				break;
		}
		confirmChoice(selectedTargets, s);
	}
	public void AIAgresiveDecision(FighterTeam hisTeam, FighterTeam vsTeam){
		Skill s = PickRandomSkillAvaible();
		Fighter[] selectedTargets = {null};
		switch(s.WhoAffects()){
			case 0:
				GD.Print("Affects his team");
				selectedTargets = PickRandomFighters(s,hisTeam);
				break;
			case 1:
				GD.Print("Affects his enemies");
				selectedTargets = PickAgresiveFighters(s,vsTeam);
				break;
			case 2:
				GD.Print("Can affect either both teams");
				Random rand = new Random();
				int i = rand.Next(0,2);
				if(i == 0)	selectedTargets = PickRandomFighters(s,hisTeam);
				else	selectedTargets = PickAgresiveFighters(s,vsTeam);
				break;
			case 3:
				GD.Print("Affects his team");
				if(s.AffectsAllTeam())
					selectedTargets = PickAllFightersAvaible(hisTeam.AllFightersAvailable(), hisTeam.GetFighters());
				else{
					selectedTargets = new Fighter[1];
					selectedTargets[0] = owner;
				}
				break;
		}
		confirmChoice(selectedTargets, s);
	}
	public void AITacticDecision(FighterTeam hisTeam, FighterTeam vsTeam){
		int bestScore = int.MinValue;
		Skill bestSkill = null;
		Fighter[] bestTargets = null;
		List<int> availableSkills = GetAvailableSkillsIndexes();
		foreach (int i in availableSkills){
			Skill skill = skills[i];
			List<Fighter> possibleTargets;
			switch (skill.WhoAffects()){
				case 0:
					possibleTargets = hisTeam.GetFighters();
					break;
				case 1:
					possibleTargets = vsTeam.GetFighters();
					break;
				case 2:
					possibleTargets = hisTeam.GetFighters(); 
					possibleTargets.AddRange(vsTeam.GetFighters());
					break;
				case 3:
					if(skill.AffectsAllTeam())
						possibleTargets = hisTeam.GetFighters();
					else{
						possibleTargets = new List<Fighter>();
						possibleTargets.Add(owner);
					}
					break;
				default:
					if(skill.AffectsAllTeam())
						possibleTargets = hisTeam.GetFighters();
					else{
						possibleTargets = new List<Fighter>();
						possibleTargets.Add(owner);
					}
					break;
			}
			int score = 0;
			Fighter[] selecting = new Fighter[1];
			if(skill.AffectsAllTeam()){
				switch (skill.WhoAffects()){
					case 0:case 1:case 3: default:
						selecting = possibleTargets.ToArray();
						score = ScoreSkill(skill, selecting);
						break;
					case 2:
						selecting = hisTeam.GetFighters().ToArray();
						score = ScoreSkill(skill, selecting);
						Fighter[] selecting2 = vsTeam.GetFighters().ToArray();
						int o = ScoreSkill(skill, selecting2);
						if(o >= score){
							score = o;
							selecting = selecting2;
						}
						break;
						
				}
			}else{
				List<Fighter[]> combinations = GenerateTargetCombinations(possibleTargets, skill.TargetsCount);
				foreach (Fighter[] combo in combinations){
					int o = ScoreSkill(skill, combo);
					if(o > score){
						score = o;
						selecting = combo;
					}
				}
			}
			if (score > bestScore){
				bestScore = score;
				bestSkill = skill;
				bestTargets = selecting;
			}
		}
		if (bestSkill == null || !bestSkill.EnoughMana(owner)){
			bestSkill = skills[1]; // skill defensiva
			bestTargets = new Fighter[] { owner };
		}
		PrepareCooldowns(bestSkill);
		confirmChoice(bestTargets, bestSkill);
	}
	public void confirmChoice(Fighter[] targets, Skill skill){
		customSignals.EmitSignal(nameof(CustomSignals.OnTargetsConfirmed),owner,targets,skill);
	}
	
	public Fighter[] PickDebugFighter(Skill s, FighterTeam team){
		List<Fighter> list = team.GetFighters();
		Fighter[] selectedTargets = new Fighter[1];
		selectedTargets[0] = list[1];
		return selectedTargets;
	}
	
	public Fighter[] PickRandomFighters(Skill s, FighterTeam team){
		Random rand = new Random();
		Fighter[] selectedTargets;
		bool[] selectedIndexes;
		List<Fighter> list = team.GetFighters();
		int target_Count, max_targets = team.AllFightersAvailable();
		if(s.AffectsAllTeam()){
			selectedTargets = PickAllFightersAvaible(max_targets,list);
		}else{
			target_Count = s.TargetsCount;
			if(target_Count > max_targets) target_Count = max_targets;
			selectedTargets = new Fighter[target_Count];
			selectedIndexes = new bool[list.Count];
			int i = 0, j = 0;
			while(target_Count > 0){
				j = rand.Next(0, list.Count);
				while(selectedIndexes[j] || list[j].IsDead()){
					j++;
					if(j == list.Count) j = 0;
				}
				selectedIndexes[j] = true;
				selectedTargets[i] = list[j];  
				i++;
				target_Count--;
			}
		}
		return selectedTargets;
	}
	public Fighter[] PickAgresiveFighters(Skill s, FighterTeam team){
		Fighter[] selectedTargets;
		List<Fighter> list = team.GetFighters();
		int target_Count, max_targets = team.AllFightersAvailable();
		if(s.AffectsAllTeam()){
			selectedTargets = PickAllFightersAvaible(max_targets,list);
		}else{
			target_Count = s.TargetsCount;
			if(target_Count > max_targets) target_Count = max_targets;
			selectedTargets = new Fighter[target_Count];
			int i = 0, j = 0;
			while(target_Count > 0 && i < fighterLastHitMe.Count){
				if(!fighterLastHitMe[i].IsDead()){
					GD.Print($"I hate on {fighterLastHitMe[i].GetEntityData().Name}");
					selectedTargets[j] = fighterLastHitMe[i];
					j++;
					target_Count--;
				}
				i++;
			}
			while(target_Count > 0){
				Fighter[] randoms = PickRandomFighters(s, team);
				foreach(Fighter f in randoms){
					if(!selectedTargets.Contains(f) && !f.IsDead()){
						selectedTargets[j++] = f;
						target_Count--;
						if(target_Count == 0) break;
					}
				}
			}
		}
		return selectedTargets;
	}
	public Fighter[] PickAllFightersAvaible(int max_targets, List<Fighter> list){
		int r = max_targets, k = 0, i = 0;
		Fighter[] selectedTargets = new Fighter[max_targets];
		while(r > 0){
			while(list[k].IsDead()){
				k++;
				if(k == list.Count)
					k = 0;
			}
			selectedTargets[i] = list[k];
			i++;
			r--;
		}
		return selectedTargets;
	}
	public Skill PickRandomSkillAvaible(){
		Random rand = new Random();
		Skill skill;
		int num = rand.Next(0, skills.Count);
		switch(num){
			case 0:
				skill = skills[0];
				break;
			case 1:
				if(owner.IsBlocked())	skill = skills[0];
				else	skill = skills[1];
				break;
			case 2:case 3:case 4:case 5:
				if(owner.IsSealed())	skill = skills[0];
				else{
					skill = skills[num];
					if(!skill.EnoughMana(owner) || Cooldowns[num] >= 0){
						if(!owner.IsBlocked())	skill = skills[1];
						else	skill = skills[0];
					}
				}
				break;
			default:
				skill = skills[0];
				break;
		}
		PrepareCooldowns(skill);
		return skill;
	}
	
	private int ScoreSkill(Skill skill, Fighter[] potentialTargets){
		int score = 0;
		// Bonus si puede matar un objetivo
		foreach (Fighter f in potentialTargets){
			if (f.IsDead()) continue;
			int estimatedDamage = Skill.CalculateDamage(skill.GivePower(), owner, f);
			if (estimatedDamage >= f.GetEntityData().giveHP()){
				score += 50;
			} else {
				score += estimatedDamage / 10;
			}
		}
		// Bonus si cura aliados críticos
		if (skill.Heals()){
			foreach (Fighter f in potentialTargets){
				if (!f.IsDead() && f.GetEntityData().giveHP() < f.GetEntityData().giveMAXHP() / 3){
					score += 30;
				}
			}
		}
		// Bonus si es buff para sí mismo y no tiene ya ese buff
		if (skill.Buffs() && skill.WhoAffects() == 3){
			foreach(int p in skill.StatusThatCanApply()){
				if(!owner.GetEffectController().HasEffectInt(p))
					score += 20;
			}
		}
		// Penaliza skills poco útiles
		if (skill.AffectsAllTeam() && potentialTargets.All(f => f.IsDead())){
			score -= 100;
		}
		return score;
	}
	
	public List<int> GetAvailableSkillsIndexes(){
		List<int> availableSkills = new List<int>();
		for (int i = 0; i < skills.Count; i++){
			if ((i >= 2 && (owner.IsSealed() || !skills[i].EnoughMana(owner) || Cooldowns[i] > 0)) || 
				(i == 1 && owner.IsBlocked())) continue;
			availableSkills.Add(i);
		}
		return availableSkills;
	}
	
	public List<Fighter[]> GenerateTargetCombinations(List<Fighter> allTargets, int selectCount){
		List<Fighter[]> results = new List<Fighter[]>();
		Fighter[] current = new Fighter[selectCount];
		Combine(allTargets, 0, 0, current, results);
		return results;
	}

	private void Combine(List<Fighter> allTargets, int start, int depth, Fighter[] current, List<Fighter[]> results){
		if (depth == current.Length){
			results.Add((Fighter[])current.Clone()); // Copiamos el arreglo actual
			return;
		}
		for (int i = start; i < allTargets.Count; i++){
			current[depth] = allTargets[i];
			Combine(allTargets, i + 1, depth + 1, current, results);
		}
	}
	
	public void GotHitByFighter(Fighter f){
		GD.Print($"Got hit by {f.GetEntityData().Name}");
		if(f.IsPlayerControlled()){
			fighterLastHitMe.Remove(f);
			fighterLastHitMe.Insert(0, f); 	
		} 
	}
	
	public void PrepareCooldowns(Skill c){
		int i = 0;
		foreach(Skill s in skills){
			if(c == s){
				Cooldowns[i] = c.CooldownTurns;
			}else{
				Cooldowns[i]--;
			}
		}
	}
	
	public void GiveFighterSkills(List<Skill> s){
		skills = s;
		Cooldowns = new int[s.Count];
		for(int i = 0; i < Cooldowns.Length;  i++)
			Cooldowns[i] = 0;
	}
	
	public void FreeAIManager(){
		fighterLastHitMe.Clear();
		skills.Clear();
		QueueFree();
	}
}
