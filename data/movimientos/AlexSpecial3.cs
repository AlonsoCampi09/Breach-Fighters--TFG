using Godot;
using System;

[GlobalClass]
public partial class AlexSpecial3 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		if(target.IsWeaving()){
			if(Skill.ProducesEffect(50)){
				target.TakeDamage(dañoTotal, caster);
				return true;
			}else{
				target.DodgedAttack();
				weaved = true;
				return true;
			}
		}
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
			if(protection || weaved){
				protection = false;
				weaved = false;
				return false;
			}
			else
				return true;
		}
		else
			return false;
	}
	public override bool DeBuffs() => Level >= RequiredLevelToEvolve;
}
