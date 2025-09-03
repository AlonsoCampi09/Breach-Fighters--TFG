using Godot;
using System;

[GlobalClass]
public partial class SlimeBasicSkill : Skill{
	
	[Export] public int slime = 0;
	
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
		if(ProducesEffect(20)){
			switch(slime){
				case 0:
					status = new StatusEffect(StatusEffectType.DeBuffVEL, 1, 20, false);
					target.ApplyStatus(status);
					break;
				case 1:
					status = new StatusEffect(StatusEffectType.DeBuffDMG, 1, 20, false);
					target.ApplyStatus(status);
					break;
				case 2:
					status = new StatusEffect(StatusEffectType.DeBuffDEF, 1, 20, false);
					target.ApplyStatus(status);
					break;
				case 3:
					status = new StatusEffect(StatusEffectType.Bloqueo, 1, 0, false);
					target.ApplyStatus(status);
					break;
			}
			return true;
		}
		return false;
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
