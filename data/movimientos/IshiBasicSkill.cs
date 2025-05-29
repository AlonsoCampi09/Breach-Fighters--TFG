using Godot;
using System;

[GlobalClass]
public partial class IshiBasicSkill : Skill{
	
	private int dañoRealizadoAnteriormente;
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		dañoRealizadoAnteriormente = 0;
		dañoRealizadoAnteriormente = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoRealizadoAnteriormente, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		if(target.IsMarked()){
			int aux = (int) dañoRealizadoAnteriormente/2;
			caster.Heal(aux);
			return true;
		}else{
			return false;
		}
	}
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
	
}
