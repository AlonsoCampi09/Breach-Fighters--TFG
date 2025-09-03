using Godot;
using System;

[GlobalClass]
public partial class VylsBasicSkill : Skill{
	
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = 0;
		if(Level >= RequiredLevelToEvolve){
			dañoTotal = CalculateDamageIgnoringTargetsDEF(finalPower,caster);
			if(target.IsProtecting()){
				target.TakeDamage(0, caster);
				protection = true;
				return true;
			}
			target.TakeDamage(dañoTotal, caster);
			return true;
		}
		else{
			dañoTotal = CalculateDamage(finalPower, caster, target);
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
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
}
