using Godot;
using System;

[GlobalClass]
public partial class AlexSpecial4 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect[] status = new StatusEffect[2];
		status[0] = new StatusEffect(StatusEffectType.BuffDMG, 2, 10, true);
		status[1] = new StatusEffect(StatusEffectType.Creacion, 2, 0, true);
		target.ApplyMultipleStatus(status);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	
	public override bool HasSecondaryEffect(){
		return false;
	}
	public override bool Buffs() => true;
}
