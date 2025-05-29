using Godot;
using System;

[GlobalClass]
public partial class IshiSpecial3 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		if(Level >= RequiredLevelToEvolve){
			StatusEffect[] status = new StatusEffect[3];
			status[0] = new StatusEffect(StatusEffectType.BuffDMG, 3, 50, true);
			status[1] = new StatusEffect(StatusEffectType.Sellado, 3, 0, false);
			status[2] = new StatusEffect(StatusEffectType.Bloqueo, 3, 0, false);
			caster.ApplyMultipleStatus(status);
			//this.putEffectsOnTargets(100, Estado.BuffDMG, 3, 50);
		}else{
			//this.putEffectsOnTargets(100, Estado.BuffDMG, 3, 30);
			StatusEffect[] status = new StatusEffect[2];
			status[0] = new StatusEffect(StatusEffectType.BuffDMG, 3, 30, true);
			status[1] = new StatusEffect(StatusEffectType.DeBuffDEF, 3, 15, false);
			caster.ApplyMultipleStatus(status);
		}
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	public override bool Buffs() => false;
}
