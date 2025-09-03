using Godot;
using System;

[GlobalClass]
public partial class EliteSlimeBasicSkill : Skill{
	
	[Export] public int slime = 0;
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		StatusEffect status;
		if(ProducesEffect(30) && slime != 1 && slime != 2){
			switch(slime){
				case 0:
					status = new StatusEffect(StatusEffectType.Aturdido, 1, 20, false);
					target.ApplyStatus(status);
					break;
				case 3:
					status = new StatusEffect(StatusEffectType.Sellado, 1, 0, false);
					target.ApplyStatus(status);
					break;
			}
			return true;
		}
		return false;
	}
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
	public override bool AffectsAllTeam(){
		if(Level >= RequiredLevelToEvolve && slime == 1)
			return true;
		else
			return false;
	}
	public override bool DeBuffs() => Level >= RequiredLevelToEvolve;
}
