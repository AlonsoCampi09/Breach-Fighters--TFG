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
				selectedTargets = PickRandomFighters(s,hisTeam);
				break;
			case 1:
				selectedTargets = PickRandomFighters(s,vsTeam);
				break;
			case 2:
				Random rand = new Random();
				int i = rand.Next(0,2);
				if(i == 0)	selectedTargets = PickRandomFighters(s,hisTeam);
				else	selectedTargets = PickRandomFighters(s,vsTeam);
				break;
			case 3:
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
				selectedTargets = PickRandomFighters(s,hisTeam);
				break;
			case 1:
				selectedTargets = PickAgresiveFighters(s,vsTeam);
				break;
			case 2:
				Random rand = new Random();
				int i = rand.Next(0,2);
				if(i == 0)	selectedTargets = PickRandomFighters(s,hisTeam);
				else	selectedTargets = PickAgresiveFighters(s,vsTeam);
				break;
			case 3:
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
	public void AITacticDecision(FighterTeam hisTeam, FighterTeam vsTeam) {
		GD.Print("TACTIC DECISION");
		int bestScore = int.MinValue;
		Skill bestSkill = null;
		Fighter[] bestTargets = null;
		List<int> availableSkills = GetAvailableSkillsIndexes();
		foreach (int idx in availableSkills) {
			Skill skill = skills[idx];
			List<Fighter> possibleTargets = null;
			switch (skill.WhoAffects()) {
				case 0: possibleTargets = hisTeam.GetFighters().Where(f => !f.IsDead()).ToList(); break;
				case 1: possibleTargets = vsTeam.GetFighters().Where(f => !f.IsDead()).ToList(); break;
				case 2:
					possibleTargets = hisTeam.GetFighters().Where(f => !f.IsDead()).ToList();
					possibleTargets.AddRange(vsTeam.GetFighters().Where(f => !f.IsDead()));
					break;
				case 3:
					possibleTargets = skill.AffectsAllTeam()
						? hisTeam.GetFighters().Where(f => !f.IsDead()).ToList()
						: new List<Fighter> { owner };
					break;
				default:
					possibleTargets = hisTeam.GetFighters().Where(f => !f.IsDead()).ToList();
					break;
			}
			GD.Print($"Posible targets for {skill.GiveTitulo()}: {string.Join(", ", possibleTargets.Select(x=>x.GetEntityData().Name))}");
			int bestScoreForSkill = int.MinValue;
			Fighter[] bestComboForSkill = null;
			if (skill.AffectsAllTeam()) {
				Fighter[] combo = possibleTargets.ToArray();
				int sc = ScoreSkill2(skill, combo);
				bestScoreForSkill = sc;
				bestComboForSkill = combo;
			} else {
				List<Fighter[]> combos = GenerateTargetCombinations(possibleTargets, skill.TargetsCount);
				if (combos.Count == 0) {
					foreach (var p in possibleTargets) {
						int sc = ScoreSkill2(skill, new Fighter[]{ p });
						if (sc > bestScoreForSkill) { bestScoreForSkill = sc; bestComboForSkill = new Fighter[]{ p }; }
					}
				} else {
					foreach (var combo in combos) {
						int sc = ScoreSkill2(skill, combo);
						if (sc > bestScoreForSkill) { bestScoreForSkill = sc; bestComboForSkill = combo; }
					}
				}
			}
			if (bestScoreForSkill > bestScore) {
				bestScore = bestScoreForSkill;
				bestSkill = skill;
				bestTargets = bestComboForSkill;
			}
		}
		if (bestSkill == null || !bestSkill.EnoughMana(owner)) {
			bestSkill = skills.FirstOrDefault(s => s.Name == "Guard") ?? skills[0]; // fallback
			bestTargets = new Fighter[]{ owner };
		}
		GD.Print($"MY BEST SKILL {bestSkill.GiveTitulo()} to { (bestTargets==null? "null": string.Join(",", bestTargets.Select(x=>x.GetEntityData().Name))) }");
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
	
	private int ScoreSkill2(Skill skill, Fighter[] targets) {
		if (skill == null || targets == null || targets.Length == 0) {
			GD.Print("ScoreSkill: skill null o sin targets");
			return int.MinValue / 2;
		}
		if (!skill.EnoughMana(owner)) {
			GD.Print($"{skill.GiveTitulo()} not enough mana -> low score");
			return int.MinValue / 2;
		}
		GD.Print($"=== Evaluando skill {skill.GiveTitulo()} ===");
		GD.Print($"Targets: {string.Join(", ", targets.Select(t => t.GetEntityData().Name))}");
		int score = 0;
		bool offensive = skill.Hurts;
		foreach (Fighter t in targets) {
			if (t.IsDead()) {
				continue;
			}
			int hp = t.GetEntityData().giveHP();
			int maxHp = t.GetEntityData().giveMAXHP();
			int Mp = t.GetEntityData().giveMP();
			int maxMp = t.GetEntityData().giveMAXMP();
			int dmg = 0;
			if (offensive) {
				dmg = EstimateDamage(skill, owner, t);
				score += dmg * 3; // peso base
				if (dmg >= hp) {
					score += 150;
				}
			} else {
				if(skill.Guard){
					int missingMp = maxMp - Mp;
					score += missingMp+5;
				}
				if(skill.Heals()){
					int missingHp = maxHp - hp;
					score += missingHp * 2;
				}
			}
			if (skill.Buffs() || skill.DeBuffs()) {
				foreach(int status in skill.StatusThatCanApply()){
					if(!t.GetEffectController().HasEffectInt(status)){
						score += 30;
					}
				}
			}
		}
		// penalización por coste de maná
		int mana = owner.GetEntityData().giveMP();
		int cost = skill.GiveCost();
		score -= cost;
		GD.Print($"TOTAL score para {skill.GiveTitulo()} = {score}");
		GD.Print("============================================");
		return score;
	}
	
	private int EstimateDamage(Skill skill, Fighter caster, Fighter target) {
		try {
			int p = skill.GivePower();
			return Skill.CalculateDamage(p, caster, target);
		}
		catch (Exception e) {
			GD.Print($"Error en EstimateDamage: {e.Message}");
			return 0;
		}
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
	
	public List<Fighter[]> GenerateTargetCombinations(List<Fighter> allTargets, int selectCount) {
		List<Fighter[]> results = new List<Fighter[]>();
		if (selectCount <= 0) return results;
		Fighter[] cur = new Fighter[selectCount];
		CombineNoRepeat(allTargets, 0, 0, cur, results);
		return results;
	}

	private void CombineNoRepeat(List<Fighter> allTargets, int start, int depth, Fighter[] cur, List<Fighter[]> results) {
		if (depth == cur.Length) {
			results.Add((Fighter[])cur.Clone());
			return;
		}
		for (int i = start; i < allTargets.Count; i++) {
			cur[depth] = allTargets[i];
			CombineNoRepeat(allTargets, i + 1, depth + 1, cur, results);
		}
	}
	public void GotHitByFighter(Fighter f){
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
