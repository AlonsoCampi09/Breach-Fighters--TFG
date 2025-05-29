using Godot;
using System;

[GlobalClass]
public partial class IshiSpecial4 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		if(Level >= RequiredLevelToEvolve){
			status = new StatusEffect(StatusEffectType.Sangrado, 3, 0, false);
		}else{
			status = new StatusEffect(StatusEffectType.Sangrado, 2, 0, false);
		}
		target.ApplyStatus(status);
		return true;
	}
	
	public override bool HasSecondaryEffect(){
		return true;
	}
	
}
