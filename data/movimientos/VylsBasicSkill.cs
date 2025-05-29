using Godot;
using System;

[GlobalClass]
public partial class VylsBasicSkill : Skill{
	
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = 0;
		if(Level >= RequiredLevelToEvolve)
			dañoTotal = CalculateDamageIgnoringTargetsDEF(finalPower,caster);
		else
			dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
}
