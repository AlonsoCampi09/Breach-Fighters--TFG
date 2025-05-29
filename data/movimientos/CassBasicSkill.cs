using Godot;
using System;

[GlobalClass]
public partial class CassBasicSkill : Skill{
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		//caster gana 1/6*ManaMac
		int aux = (int)caster.GetEntityData().giveMAXMP()/6;
		caster.RestoreMana(aux);
		return true;
	}
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
}
