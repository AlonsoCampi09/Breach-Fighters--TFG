using Godot;
using System;

[GlobalClass]
public partial class CassSpecial2 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect[] status= new StatusEffect[2];
		if(Level >= RequiredLevelToEvolve){
			status[0] = new StatusEffect(StatusEffectType.BuffDMG, 3, 30, true);
		status[1] = new StatusEffect(StatusEffectType.BuffDEF, 3, 30, true);
		}
		else{
			status[0] = new StatusEffect(StatusEffectType.BuffDMG, 3, 25, true);
		status[1] = new StatusEffect(StatusEffectType.BuffDEF, 3, 25, true);
		}
		caster.ApplyMultipleStatus(status);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	public override bool Buffs() => true;
}
