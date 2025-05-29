using Godot;
using System;

[GlobalClass]
public partial class AlexSpecial1 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect[] status = new StatusEffect[2];
		status[0] = new StatusEffect(StatusEffectType.BuffDMG, 2, 20, true);
		status[1] = new StatusEffect(StatusEffectType.BuffDEF, 2, 20, true);
		target.ApplyMultipleStatus(status);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect[] status = new StatusEffect[2];
		status[0] = new StatusEffect(StatusEffectType.BuffDMG, 3, 20, true);
		status[1] = new StatusEffect(StatusEffectType.BuffDEF, 3, 20, true);
		caster.ApplyMultipleStatus(status);
		return true;
	}
	
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve){
			return true;
		}
		else{
			return false;
		}
	}
	public override bool BuffsOnlyTeammates() => true;
}
