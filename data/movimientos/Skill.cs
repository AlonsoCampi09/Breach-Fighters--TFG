using Godot;
using System;

public interface ISkillDisplayData {
	string DisplayName { get; }
	string DisplayDescription { get; }
	int ManaCost { get; }
	bool IsUnlocked { get; }
}

[GlobalClass]
public abstract partial class Skill : Resource, ISkillDisplayData{
	public enum Effect_Obj {Ally, Enemy, Both, Self}

	[Export]public int Level = 1;
	

	[Export] public string Name;
	[Export] public string Name_Evolved;
	[Export] public string Description;
	[Export] public string Description_Evolved;
	
	[Export] public int BasePower;
	[Export] public int BasePower_Evolved;
	[Export] public int Cost;
	[Export] public int Cost_Evolved;
	[Export] public int RequiredLevelToEvolve = 0;
	[Export] public int RequiredLevelToUnlock = 0;
	[Export] public Effect_Obj effectObj;
	[Export] public int TargetsCount = 1;
	[Export] public bool TargetsAllTeam = false;
	[Export] public bool Hurts = false;
	
	[Export] public int Limit = 0;
	[Export] public int Guaranteed = 0;
	[Export] public int Limit_Evolved = 0;
	[Export] public int Guaranteed_Evolved = 0;
	[Export] public int Percentage = 0;
	[Export] public int Percentage_Evolved = 0;
	[Export] public int Count = 0;
	
	[Export] public int CooldownTurns = 0;
	
	[Export] public int[] StatusCanApply;
	[Export] public int[] StatusCanApplyEvolved;
	
	private bool IsEvolved => Level >= RequiredLevelToEvolve;
	public int Cooldown => CooldownTurns;
	
	
	public abstract bool Execute1(Fighter caster, Fighter target, BattleManager battle);
	public abstract bool Execute2(Fighter caster, Fighter target, BattleManager battle);
	
	public int WhoAffects(){
		switch(effectObj){
			case Effect_Obj.Ally:
				return 0;
			case Effect_Obj.Enemy:
				return 1;
			case Effect_Obj.Both:
				return 2;
			case Effect_Obj.Self:
				return 3;
		}
		return -1;
	}
	public static int CalculateDamage(int p, Fighter caster, Fighter target){
		int formula = 0, dañoBufado,  defensaBufado, ATQOrigen, DEFOrigen, f1, f2;
		float porcentajeATQ, porcentajeDEF;
		
		porcentajeATQ = 1  + (caster.GetEntityData().giveDMGBuf() - caster.GetEntityData().giveDMGDeBuf()) / 100;
		ATQOrigen = caster.GetEntityData().giveDMG();
		dañoBufado = (int) (ATQOrigen * porcentajeATQ);
		f1 = ATQOrigen + dañoBufado;
	
		porcentajeDEF  = 1  + (target.GetEntityData().giveDEFBuf() - target.GetEntityData().giveDEFDeBuf()) / 100;
		DEFOrigen = target.GetEntityData().giveDEF();
		defensaBufado = (int) (DEFOrigen * porcentajeDEF);
		f2 = DEFOrigen + defensaBufado;
		formula = Math.Max(1,p+f1-f2);
		
		return formula;
	}
	public static int CalculateDamageIgnoringTargetsDEF(int p, Fighter caster){
		int formula = 0, dañoBufado, ATQOrigen, f1;
		float porcentajeATQ;
		
		porcentajeATQ = 1  + (caster.GetEntityData().giveDMGBuf() - caster.GetEntityData().giveDMGDeBuf()) / 100;
		ATQOrigen = caster.GetEntityData().giveDMG();
		dañoBufado = (int) (ATQOrigen * porcentajeATQ);
		f1 = ATQOrigen + dañoBufado;
		
		formula = Math.Max(1,p+f1);
		
		return formula;
	}
	public static bool ProducesEffect(double proba){
		Random rand = new Random();
		int num_max = 100, actual_prob, random_number;
		double aux = proba;
		while(aux < 1){
			aux *= 10;
			num_max *= 10;
		}
		actual_prob = (int) aux;
		random_number = rand.Next(1, num_max+1);
		if(actual_prob + random_number > num_max){
			return true;
		}
		return false;
	}
	
	public int[] StatusThatCanApply() => IsEvolved ? StatusCanApplyEvolved : StatusCanApply;
	public string GiveTitulo() => IsEvolved ? Name_Evolved : Name;
	public string GiveDescription() => IsEvolved ? Description_Evolved : Description;
	public int GivePower() => IsEvolved ? BasePower_Evolved : BasePower;
	public int GiveCost() => IsEvolved ? Cost_Evolved : Cost;
	public int GiveLimit() => IsEvolved ? Limit_Evolved : Limit;
	public int GiveGuaranteed() => IsEvolved ? Guaranteed_Evolved : Guaranteed;
	public int GivePercentage() => IsEvolved ? Percentage_Evolved : Percentage;
	
	public string DisplayName => GiveTitulo();
	public string DisplayDescription => GiveDescription();
	public int ManaCost => GiveCost();
	public int Power => GivePower();
	public bool IsUnlocked => MoveIsAvailable();
	
	public bool EnoughMana(Fighter caster){
		return caster.GetEntityData().giveMP() >= GiveCost();
	}
	public virtual bool AffectsAllTeam(){
		return TargetsAllTeam;
	}
	public bool MoveIsAvailable(){
		return Level >= RequiredLevelToUnlock;
	}
	public virtual void AssingLevel(int l){
		this.Level = l;
	}
	public void LevelUp(){
		this.Level++;
	}
	public virtual bool HasSecondaryEffect(){
		return false;
	}
	public virtual bool Heals() => false;
	public virtual bool HealsOnlyTeammates() => false;
	public virtual bool Buffs() => false;
	public virtual bool BuffsOnlyTeammates() => false;
	public virtual bool DeBuffs() => false;
	public virtual bool Cleanse() => false;
}
