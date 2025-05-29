using Godot;
using System;

[GlobalClass]
public partial class VylsSpecial1 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		if(Level >= RequiredLevelToEvolve){
			StatusEffect[] status = new StatusEffect[2];
			status[0] = new StatusEffect(StatusEffectType.Regeneracion, 3, 0, true);
			status[1] = new StatusEffect(StatusEffectType.Energetico, 3, 0, true);
			target.ApplyMultipleStatus(status);
		}else{
			StatusEffect status = new StatusEffect(StatusEffectType.Regeneracion, 3, 0, true);
			target.ApplyStatus(status);
		}
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	public override bool Heals() => true;
	public override bool Buffs() => true;
}
