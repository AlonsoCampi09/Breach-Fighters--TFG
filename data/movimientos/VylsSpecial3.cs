using Godot;
using System;

[GlobalClass]
public partial class VylsSpecial3 : Skill{
	private bool aux = false;
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		if(!target.GetEntityData().isControlled()){
			aux = true; 
			int finalPower = GivePower();
			int dañoTotal = CalculateDamage(finalPower, caster, target);
			//Target Receives Damage;
			target.TakeDamage(dañoTotal, caster);
		}else{
			if(Level >= RequiredLevelToEvolve){
				//Restauracion
				target.RestoreGoodStatus();
			}
			else{
				//Reincio
				target.ResetStatus();
			}
		}
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		target.ResetStatus();
		return true;
	}
	public override bool HasSecondaryEffect(){
		return aux;
	}
	public override bool Cleanse() => true;
}
