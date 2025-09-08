using Godot;
using System;

[GlobalClass]
public partial class DreamerSpecial : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		if(Level >= RequiredLevelToEvolve){
			int n = (int) (caster.GetEntityData().giveMAXHP()*0.3);
			target.HealAction(n);
			
		}else{
			StatusEffect status = new StatusEffect(StatusEffectType.Regeneracion, 3, 0, true);
			target.ApplyStatus(status);
		}
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status = new StatusEffect(StatusEffectType.Energetico, 3, 0, true);
		target.ApplyStatus(status);
		return true;
	}
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve){
			return true;
		}
		else
			return false;
	}
	public override bool Heals(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
	public override bool Buffs() => true;
}
