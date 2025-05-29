using Godot;
using System;

[GlobalClass]
public partial class SlimeBasicSkill : Skill{
	
	[Export] public int slime = 0;
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		if(ProducesEffect(10)){
			switch(slime){
				case 0:
					//this.putEffectsOnTargets(10, Estado.DeBuffVEL, 1, 20);
					break;
				case 1:
					//this.putEffectsOnTargets(10, Estado.DeBuffDMG, 1, 10);
					break;
				case 2:
					//this.putEffectsOnTargets(10, Estado.DeBuffDEF, 1, 10);
					break;
				case 3:
					//this.putEffectsOnTargets(10, Estado.Sellado, 1, 0);
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
	public override bool DeBuffs() => Level >= RequiredLevelToEvolve;
}
