using Godot;
using System;

[GlobalClass]
public partial class IshiGuardSkill : Skill{
	
	int recuperacion_MP, def_ptg;
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		def_ptg = 35;
		recuperacion_MP = (int) caster.GetEntityData().giveMAXMP()/3;
		
		caster.GetEntityData().restoreMP(recuperacion_MP);
		status = new StatusEffect(StatusEffectType.BuffDEF, 2, def_ptg, true);
		
		caster.ApplyStatus(status);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		status = new StatusEffect(StatusEffectType.Evasion, 2, 0, true);
		caster.ApplyStatus(status);
		return true;
	}
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
}
