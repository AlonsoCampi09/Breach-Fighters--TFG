using Godot;
using System;

[GlobalClass]
public partial class CassSpecial3 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		//Target Receives Damage;
		target.TakeDamage(dañoTotal, caster);
		return true;
		
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		if(Level >= RequiredLevelToEvolve){
			//Gets Sealed for 2 turns
			StatusEffect status;
			status = new StatusEffect(StatusEffectType.Sellado, 2, 0, false);
			
			caster.ApplyStatus(status);
		}
		else{
			int aux = (int) caster.GetEntityData().giveMAXMP()/4;
			target.LosesMana(aux);
		}
		return true;
	}
	public override bool HasSecondaryEffect(){
		return true;
	}
	public override bool DeBuffs() => true;
}
