using Godot;
using System;

[GlobalClass]
public partial class AlexSpecial3 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		if(ProducesEffect(30)){
			status = new StatusEffect(StatusEffectType.Aturdido, 1, 0, false);
	
			target.ApplyStatus(status);
			return true;
		}else{
			return false;
		}
	}
	
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve){
			return true;
		}
		else{
			return false;
		}
	}
	public override bool DeBuffs() => Level >= RequiredLevelToEvolve;
}
