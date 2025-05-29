using Godot;
using System;

[GlobalClass]
public partial class IshiSpecial1 : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeMultipleHits(dañoTotal, GivePercentage(), GiveGuaranteed(), GiveLimit(), caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
}
