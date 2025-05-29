using Godot;
using System;

[GlobalClass]
public partial class VylsSpecial4 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		if(Level >= RequiredLevelToEvolve){
			//VANGUARDIA
			status = new StatusEffect(StatusEffectType.Vanguardia, 2, 0, true);
		}
		else{
			//DEF 50 2
			status = new StatusEffect(StatusEffectType.BuffDEF, 2, 50, true);
		}
		target.ApplyStatus(status);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	public override bool BuffsOnlyTeammates() => true;
}
