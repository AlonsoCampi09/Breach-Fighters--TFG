using Godot;
using System;

[GlobalClass]
public partial class AlexSpecial2 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		//Target receives a DebuffDEF 10 for 2 turn;
		StatusEffect status;
		status = new StatusEffect(StatusEffectType.DeBuffDEF, 2, 20, false);
		
		target.ApplyStatus(status);
		return true;
	}
	
	public override bool HasSecondaryEffect(){
		return true;
	}
	public override bool AffectsAllTeam(){
		if(Level >= RequiredLevelToEvolve){
			return true;
		}
		else{
			return false;
		}
	}
	public override bool DeBuffs() => true;
}
