using Godot;
using System;

[GlobalClass]
public partial class CassGuardSkill : Skill{
	
	int recuperacion_MP, def_ptg;
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		if(Level >= RequiredLevelToEvolve){
			recuperacion_MP = (int) caster.GetEntityData().giveMAXMP()/2;
			def_ptg = 50;
		}else{
			recuperacion_MP = (int) caster.GetEntityData().giveMAXMP()/3;
			def_ptg = 40;
		}
		caster.GetEntityData().restoreMP(recuperacion_MP);
		status = new StatusEffect(StatusEffectType.BuffDEF, 2, def_ptg, true);
		
		caster.ApplyStatus(status);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return false;
	}
}
