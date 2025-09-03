using Godot;
using System;

[GlobalClass]
public partial class CassSpecial4 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		//Target Receives Damage;
		if(target.IsProtecting()){
			target.TakeDamage(0, caster);
			protection = true;
			return true;
		}
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		if(Level >= RequiredLevelToEvolve){
			//Targets receive a DMGdebuff 20% en su proximo turno 1
			status = new StatusEffect(StatusEffectType.DeBuffDMG, 2, 25, false);
			target.ApplyStatus(status);
			return true;
		}
		else{
			if(ProducesEffect(25)){
				status = new StatusEffect(StatusEffectType.DeBuffDMG, 2, 25, false);
				target.ApplyStatus(status);
				return true;
			}else{
				return true;
			}
		}
	}
	public override bool HasSecondaryEffect(){
		if(protection){
			protection = false;
			return false;
		}
		else
			return true;
	}
}
