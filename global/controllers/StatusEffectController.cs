using Godot;
using System;
using System.Linq;
using System.Collections.Generic;  // This is the library that includes Dictionary
using System.Threading.Tasks;

public partial class StatusEffectController : Node{
	
	private Fighter owner;
	private List<StatusEffect> activeEffects;
	
	private CustomSignals customSignals;
	
	public override void _Ready(){
		owner = GetParent() as Fighter;
		activeEffects = new List<StatusEffect>();
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}
	public StatusEffectController() {} 
	
	public void AddEffect(StatusEffect effect){
		StatusEffect existing = activeEffects.Find(e => e.Type == effect.Type);
		if (existing != null){
			if(CheckStatusMagnitude(effect)){
				if(existing.Duration <= effect.Duration)
					existing.Duration = effect.Duration;
				existing.Magnitude = effect.Magnitude;
			}
		}
		else{
			activeEffects.Add(effect);
			SortEffectsByStatus();
		}
	}
	
	public async Task ProcessEffects(BattleManager b){
		if (activeEffects.Count == 0){
			GD.Print($"There are no Status effects on {owner.GetEntityData().Name}");
		}
		else{
			int i = 0;
			while (i < activeEffects.Count && !owner.IsDead()){
				string msg = null;
				var effect = activeEffects[i];

				switch (effect.Type){
					case StatusEffectType.Regeneracion:
						owner.Heal(Math.Max(1, (int)owner.GetEntityData().giveMAXHP() / 6));
						await ToSignal(customSignals, "OnEffectIsDone");
						msg = $"{owner.GetEntityData().Name} se ha recuperado vida por {effect.StatusName.ToLower()}.";
						break;
					case StatusEffectType.Energetico:
						owner.RestoreMana(Math.Max(1, (int)owner.GetEntityData().giveMAXMP() / 6));
						await ToSignal(customSignals, "OnEffectIsDone");
						msg = $"{owner.GetEntityData().Name} ha recuperado energía por {effect.StatusName.ToLower()}.";
						break;
					case StatusEffectType.Sangrado:
					case StatusEffectType.Envenenado:
						owner.TakeDamageAnonymous(Math.Max(1, 1 + (int)owner.GetEntityData().giveMAXHP() / 12));
						await ToSignal(customSignals, "OnEffectIsDone");
						msg = $"{owner.GetEntityData().Name} sufre por {effect.StatusName.ToLower()}.";
						break;
					case StatusEffectType.Creacion:
						await b.ExplosionCastedFromFighterOnATeam(owner, 6);
						msg = owner.GetEntityData().isControlled()
							? $"El equipo contrincante sufre daño por la explosión de {effect.StatusName.ToLower()}."
							: $"Tu equipo sufre daño por la explosión de {effect.StatusName.ToLower()}.";
						break;
				}
				effect.Duration--;
				if (effect.Duration <= 0 && effect.Type != StatusEffectType.Envenenado){
					CleanupEffect(effect);
					activeEffects.Remove(effect);
					SortEffectsByStatus();
					// Note: Do NOT increment i if you removed the current effect, as the next one will now be at index i.
					continue;
				}
				if (msg != null){
					GD.Print($"msg != null : {msg}");
					customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), msg);
					await ToSignal(customSignals, "OnDialogIsOver");
				}
				i++;
			}
		}
	}
	public void SortEffectsByStatus(){
		activeEffects.Sort((a, b) => {
			if(a.LastToBeExecuted && !b.LastToBeExecuted) return 1;
			else if(!a.LastToBeExecuted && b.LastToBeExecuted) return -1;
			else if(!a.LastToBeExecuted && !b.LastToBeExecuted){
				if (a.IsBuff && !b.IsBuff) return -1;
				else if (!a.IsBuff && b.IsBuff) return 1;
				else return 0;
			}
			else return 0;
		});
	}
	
	public bool CheckStatusMagnitude(StatusEffect effect){
		bool putEffect = false;
		if(effect.Type == StatusEffectType.BuffDMG){
			putEffect = owner.GetEntityData().newATQBuffIsBetter(effect.Magnitude);
			if(putEffect)
				owner.GetEntityData().addBuffDMG(effect.Magnitude);
		} else if(effect.Type == StatusEffectType.DeBuffDMG){
			putEffect = owner.GetEntityData().newATQDeBuffIsBetter(effect.Magnitude);
			if(putEffect)
				owner.GetEntityData().addDeBuffDMG(effect.Magnitude);
		} else if(effect.Type == StatusEffectType.BuffDEF){
			putEffect = owner.GetEntityData().newDEFBuffIsBetter(effect.Magnitude);
			if(putEffect)
				owner.GetEntityData().addBuffDEF(effect.Magnitude);
		} else if(effect.Type == StatusEffectType.DeBuffDEF){
			putEffect = owner.GetEntityData().newDEFDeBuffIsBetter(effect.Magnitude);
			if(putEffect)
				owner.GetEntityData().addDeBuffDEF(effect.Magnitude);
		} else if(effect.Type == StatusEffectType.BuffVEL){
			putEffect = owner.GetEntityData().newVELBuffIsBetter(effect.Magnitude);
			if(putEffect)
				owner.GetEntityData().addBuffVEL(effect.Magnitude);
		} else if(effect.Type == StatusEffectType.DeBuffVEL){
			putEffect = owner.GetEntityData().newVELDeBuffIsBetter(effect.Magnitude);
			if(putEffect)
				owner.GetEntityData().addDeBuffVEL(effect.Magnitude);
		} else{
			putEffect = true;
		}
		return putEffect;
	}
	
	public void CleanupEffect(StatusEffect effect){
		
		switch (effect.Type){
			case StatusEffectType.BuffDMG:
				owner.GetEntityData().removeBuffDMG();
				break;
			case StatusEffectType.DeBuffDMG:
				owner.GetEntityData().removeDeBuffDMG();
				break;
			case StatusEffectType.BuffDEF:
				owner.GetEntityData().removeBuffDEF();
				break;
			case StatusEffectType.DeBuffDEF:
				owner.GetEntityData().removeDeBuffDEF();
				break;
			case StatusEffectType.BuffVEL:
				owner.GetEntityData().removeBuffVEL();
				break;
			case StatusEffectType.DeBuffVEL:
				owner.GetEntityData().removeDeBuffVEL();
				break;
		}
	}
	
	public void ClearAllEffects(){
		StatusEffect veneno = activeEffects.Find(e => e.Type == StatusEffectType.Envenenado);
		activeEffects.Clear();
		if(veneno != null)
			AddEffect(veneno);
	}
	public void ClearNegativeEffects(){
		StatusEffect veneno = activeEffects.Find(e => e.Type == StatusEffectType.Envenenado);
		activeEffects.RemoveAll(effect => !effect.IsBuff);
		if(veneno != null)
			AddEffect(veneno);
	}
	public void RemoveAllEffects(){
		activeEffects.Clear();
	}
	
	public bool HasEffect(StatusEffectType type){
		return activeEffects.Any(e => e.Type == type);
	}
	public bool HasEffectInt(int p){
		StatusEffectType type = IntToStatus(p);
		return activeEffects.Any(e => e.Type == type);
	}
	
	public List<StatusEffect> GetActiveStatus(){
		return activeEffects;
	}
	
	public bool ThereIsActiveStatus(){
		return activeEffects.Count > 0;
	}
	
	public StatusEffectType IntToStatus(int statusNumber){
		switch(statusNumber){
			case 0:
				return StatusEffectType.BuffDMG;
			case 1:
				return StatusEffectType.DeBuffDMG;
			case 2:
				return StatusEffectType.BuffDEF;
			case 3:
				return StatusEffectType.DeBuffDEF;
			case 4:
				return StatusEffectType.BuffVEL;
			case 5:
				return StatusEffectType.DeBuffVEL;
			case 6:
				return StatusEffectType.Aturdido;
			case 7:
				return StatusEffectType.Sellado;
			case 8:
				return StatusEffectType.Bloqueo;
			case 9:
				return StatusEffectType.Sangrado;
			case 10:
				return StatusEffectType.Envenenado;
			case 11:
				return StatusEffectType.Regeneracion;
			case 12:
				return StatusEffectType.Evasion;
			case 13:
				return StatusEffectType.Marca_del_cazador;
			case 14:
				return StatusEffectType.Creacion;
			case 15:
				return StatusEffectType.Vanguardia;
		}
		return StatusEffectType.None;
	}
	
	public string CurrentStatusesToString(){
		string res = "";
		foreach(StatusEffect se in activeEffects){
			res = $"{res}{se.ToString()}, ";
		}
		return res;
	}
}
