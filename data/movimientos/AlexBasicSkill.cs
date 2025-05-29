using Godot;
using System;

[GlobalClass]
public partial class AlexBasicSkill : Skill{
	
	
	public override bool Execute1(Fighter caster, Fighter target, BattleManager battle){
		int finalPower = GivePower();
		int dañoTotal = CalculateDamage(finalPower, caster, target);
		//Target Receives Damage;
		target.TakeDamage(dañoTotal, caster);
		return true;
	}
	public override bool Execute2(Fighter caster, Fighter target, BattleManager battle){
		return RandomEffect( caster, target, battle);
	}
	
	public bool RandomEffect(Fighter caster, Fighter target, BattleManager battle){
		Random rand = new Random();
		StatusEffect status;
		int num_max = 1000, random_number;
		random_number = rand.Next(1, num_max+1);
		bool happened = false;
		if(random_number == 1){
			//Caster self afflicts Stun for 2 turns
			status = new StatusEffect(StatusEffectType.Aturdido, 2, 0, false);
			caster.ApplyStatus(status);
			happened = true;
		}else if(random_number >= 250 && random_number <= 400){
			//Caster restores half of its mana
			int aux = (int) caster.GetEntityData().giveMAXMP()/2;
			caster.RestoreMana(aux);
			happened = true;
		}else if(random_number >= 550 && random_number <= 750){
			//Caster self afflicts Regeneration for 2 turns
			status = new StatusEffect(StatusEffectType.Regeneracion, 2, 0, false);
			caster.ApplyStatus(status);
			happened = true;
		}
		//Mas efectos en el futuro
		return happened;
	}
	
	public override bool HasSecondaryEffect(){
		if(Level >= RequiredLevelToEvolve)
			return true;
		else
			return false;
	}
}
