using Godot;
using System;

public enum StatusEffectType{
	BuffDMG,
	DeBuffDMG,
	BuffDEF,
	DeBuffDEF,
	BuffVEL,
	DeBuffVEL,
	Aturdido,
	Sellado,
	Bloqueo,
	Sangrado,
	Envenenado,
	Regeneracion,
	Energetico,
	Evasion,
	Marca_del_cazador,
	Creacion,
	Vanguardia,
	None
	//etc
}

public partial class StatusEffect : Resource{
	public StatusEffectType Type;
	public string StatusName;
	public int Duration; // en turnos
	public int Magnitude; // Ej. daño por turno, % velocidad, etc.
	public bool IsBuff; // Para distinguir curativos/debilitantes
	public bool Infinite; // Para saber si se puede limpiar
	public bool LastToBeExecuted; // Para saber si se puede limpiar
	
	public StatusEffect(StatusEffectType t, int d, int m, bool b){
		Type = t;
		StatusName = StatusToString(t);
		Duration = d; 
		Magnitude = m; 
		IsBuff = b;
		if(t == StatusEffectType.Envenenado){
			Infinite = true;
			LastToBeExecuted = false;
		}else if(t == StatusEffectType.Creacion){
			Infinite = false;
			LastToBeExecuted = true;
		}else{
			Infinite = false;
			LastToBeExecuted = false;
		}
	}
	public string StatusToString(StatusEffectType s){
		switch(s){
			case StatusEffectType.BuffDMG:
				return "Aumento de ataque";
			case StatusEffectType.DeBuffDMG:
				return "Disminución de ataque";
			case StatusEffectType.BuffDEF:
				return "Aumento de defensa";
			case StatusEffectType.DeBuffDEF:
				return "Disminución de defensa";
			case StatusEffectType.BuffVEL:
				return "Aumento de velocidad";
			case StatusEffectType.DeBuffVEL:
				return "Disminución de velocidad";
			case StatusEffectType.Aturdido:
				return "Aturdido";
			case StatusEffectType.Sellado:
				return "Sellado";
			case StatusEffectType.Bloqueo:
				return "Bloqueo";
			case StatusEffectType.Sangrado:
				return "Sangrado";
			case StatusEffectType.Envenenado:
				return "Envenenado";
			case StatusEffectType.Regeneracion:
				return "Regeneracion";
			case StatusEffectType.Energetico:
				return "Energetico";
			case StatusEffectType.Evasion:
				return "Evasion";
			case StatusEffectType.Marca_del_cazador:
				return "Marca del cazador";
			case StatusEffectType.Creacion:
				return "Creacion";
			case StatusEffectType.Vanguardia:
				return "Vanguardia";
			default:
				return "un estado no reconocido (espera, que?)";
		}
	}
	public override string ToString(){
		switch(Type){
			case StatusEffectType.BuffDMG:
			case StatusEffectType.DeBuffDMG:
			case StatusEffectType.BuffDEF:
			case StatusEffectType.DeBuffDEF:
			case StatusEffectType.BuffVEL:
			case StatusEffectType.DeBuffVEL:
				return $"{StatusToString(Type)} de {Magnitude}%";
			default:
				return $"{StatusToString(Type)}";
		}
	}
}
