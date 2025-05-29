using Godot;
using System;

[GlobalClass]
public partial class IshiSpecial2 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		if(Level >= RequiredLevelToEvolve){
			//Targets Def DEBUFF 20 3
			StatusEffect[] status = new StatusEffect[2];
			status[0] = new StatusEffect(StatusEffectType.DeBuffDEF, 3, 35, false);
			status[1] = new StatusEffect(StatusEffectType.Marca_del_cazador, 3, 0, false);
			target.ApplyMultipleStatus(status);
		}
		else{
			//Targets Def DEBUFF 20 3
			StatusEffect status = new StatusEffect(StatusEffectType.DeBuffDEF, 3, 35, false);
			target.ApplyStatus(status);
		}
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	public override bool DeBuffs() => true;
}
