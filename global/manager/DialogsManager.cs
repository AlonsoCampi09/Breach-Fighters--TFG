using Godot;
using System;

public partial class DialogsManager : Node{
	
	private CustomSignals customSignals;
	
	public override void _Ready(){
		customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.OnTargetsConfirmed += prepareDialog;
		customSignals.OnAffectedByMultipleEffects += prepareDialogMoveEffects;
	}
	
	public string EstadosATexto(StatusEffect s){
		switch(s.Type){
			case StatusEffectType.BuffDMG:
				return $"un aumento de ataque del {s.Magnitude} porciento";
			case StatusEffectType.DeBuffDMG:
				return $"una disminución de ataque del {s.Magnitude} porciento";
			case StatusEffectType.BuffDEF:
				return $"un aumento de defensa del {s.Magnitude} porciento";
			case StatusEffectType.DeBuffDEF:
				return $"una disminución de defensa del {s.Magnitude} porciento";
			case StatusEffectType.BuffVEL:
				return $"un aumento de velocidad del {s.Magnitude} porciento";
			case StatusEffectType.DeBuffVEL:
				return $"una disminución del de velocidad del {s.Magnitude} porciento";
			case StatusEffectType.Aturdido:
				return "aturdimiento";
			case StatusEffectType.Sellado:
				return "sello magico";
			case StatusEffectType.Bloqueo:
				return "bloqueo de defensas";
			case StatusEffectType.Sangrado:
				return "sangrado";
			case StatusEffectType.Envenenado:
				return "veneno";
			case StatusEffectType.Regeneracion:
				return "regeneracion pasiva de vida";
			case StatusEffectType.Energetico:
				return "regeneracion pasiva de maná";
			case StatusEffectType.Evasion:
				return "evasión";
			case StatusEffectType.Marca_del_cazador:
				return "la marca del cazador";
			case StatusEffectType.Creacion:
				return "el potenciador de Alex";
			case StatusEffectType.Vanguardia:
				return "la proteccion de Vyls";
			default:
				return "un estado no reconocido (espera, que?)";
		}
	}
	
	public void prepareDialog(Fighter actor, Fighter[] targets, Skill skill){
		string dialogo = actor.GetEntityData().Name  + " ha usado " + skill.GiveTitulo(); 
		if(skill.WhoAffects() != 3){
			dialogo  = $"{dialogo} en ";
			if(skill.AffectsAllTeam()){
				if(skill.WhoAffects() == 0)
					dialogo  = $"{dialogo}su equipo";
				else if(skill.WhoAffects() == 1)
					dialogo  = $"{dialogo}el equipo enemigo";
				else
					dialogo  = $"{dialogo} todos los adyacentes.";
			}else{
				dialogo  = $"{dialogo}{targets[0].GetEntityData().Name}";
				if(targets.Length > 1){
					int i = 1;
					while(i <  targets.Length - 1){
						dialogo = $"{dialogo}, {targets[i].GetEntityData().Name}";
						i++;
					}
					dialogo = $"{dialogo} y {targets[i].GetEntityData().Name}";
				}
			}
		}
		dialogo = $"{dialogo}.";
		GD.Print($"Dialogo preparado: {dialogo}");
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), dialogo);
	}
	public void prepareDialogMoveEffects(string name, StatusEffect[] effects){
		string dialogo = $"{name} ha sido afectado por {EstadosATexto(effects[0])}";
		int i = 1;
		if(effects.Length > 2){
			while(i < effects.Length-1){
				dialogo = $"{dialogo}, {EstadosATexto(effects[i])}";
				i++;
			}
		}
		dialogo = $"{dialogo} y {EstadosATexto(effects[i])}";
		dialogo = $"{dialogo}.";
		GD.Print($"Dialogo preparado: {dialogo}");
		customSignals.EmitSignal(nameof(CustomSignals.OnShowDialog), dialogo);
	}
	
}
