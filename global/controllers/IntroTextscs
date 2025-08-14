using Godot;
using System;
using System.Collections.Generic;

public partial class StatusHelper{
	private static Dictionary<StatusEffectType, Texture2D> statusIcons = new Dictionary<StatusEffectType, Texture2D>{
		{ StatusEffectType.BuffDMG, GD.Load<Texture2D>("res://assets/status/atq_buf.png") },
		{ StatusEffectType.DeBuffDMG, GD.Load<Texture2D>("res://assets/status/atq_debuf.png") },
		{ StatusEffectType.BuffDEF, GD.Load<Texture2D>("res://assets/status/def_buf.png") },
		{ StatusEffectType.DeBuffDEF, GD.Load<Texture2D>("res://assets/status/def_debuf.png") },
		{ StatusEffectType.BuffVEL, GD.Load<Texture2D>("res://assets/status/vel_buf.png") },
		{ StatusEffectType.DeBuffVEL, GD.Load<Texture2D>("res://assets/status/vel_debuf.png") },
		{ StatusEffectType.Aturdido, GD.Load<Texture2D>("res://assets/status/stunned.png") },
		{ StatusEffectType.Sellado, GD.Load<Texture2D>("res://assets/status/sealed.png") },
		{ StatusEffectType.Bloqueo, GD.Load<Texture2D>("res://assets/status/blocked.png") },
		{ StatusEffectType.Sangrado, GD.Load<Texture2D>("res://assets/status/bleeding.png") },
		{ StatusEffectType.Envenenado, GD.Load<Texture2D>("res://assets/status/poison.png") },
		{ StatusEffectType.Regeneracion, GD.Load<Texture2D>("res://assets/status/regen.png") },
		{ StatusEffectType.Energetico, GD.Load<Texture2D>("res://assets/status/energetic.png") },
		{ StatusEffectType.Evasion, GD.Load<Texture2D>("res://assets/status/evasion.png") },
		{ StatusEffectType.Marca_del_cazador, GD.Load<Texture2D>("res://assets/status/hunters_call.png") },
		{ StatusEffectType.Creacion, GD.Load<Texture2D>("res://assets/status/magic_creation.png") },
		{ StatusEffectType.Vanguardia, GD.Load<Texture2D>("res://assets/status/vanguard.png") },
		// De momento estan todos...
	};
	
	public static Texture2D GetIcono(StatusEffectType status){
		return statusIcons.TryGetValue(status, out var icono) ? icono : null;
	}
}
