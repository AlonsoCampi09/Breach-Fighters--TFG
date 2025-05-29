using Godot;
using System;

[GlobalClass]
public partial class CassSpecial1 : Skill{
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		//Target Receives Damage;
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
	public override bool AffectsAllTeam(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
}
