using Godot;
using System;
using System.Collections.Generic;

public partial class EstadoHelper
{
	private static Dictionary<Estado, Texture2D> IconosEstado = new Dictionary<Estado, Texture2D>
	{
		{ Estado.BuffDMG, GD.Load<Texture2D>("res://assets/estados/atq_buf.png") },
		{ Estado.DeBuffDMG, GD.Load<Texture2D>("res://assets/estados/atq_debuf.png") },
		{ Estado.BuffDEF, GD.Load<Texture2D>("res://assets/estados/def_buf.png") },
		{ Estado.DeBuffDEF, GD.Load<Texture2D>("res://assets/estados/def_debuf.png") },
		{ Estado.BuffVEL, GD.Load<Texture2D>("res://assets/estados/vel_buf.png") },
		{ Estado.DeBuffVEL, GD.Load<Texture2D>("res://assets/estados/vel_debuf.png") },
		{ Estado.Aturdido, GD.Load<Texture2D>("res://assets/estados/stunned.png") },
		{ Estado.Sellado, GD.Load<Texture2D>("res://assets/estados/sealed.png") },
		{ Estado.Bloqueo, GD.Load<Texture2D>("res://assets/estados/blocked.png") },
		{ Estado.Sangrado, GD.Load<Texture2D>("res://assets/estados/bleeding.png") },
		{ Estado.Envenenado, GD.Load<Texture2D>("res://assets/estados/poison.png") },
		{ Estado.Regeneracion, GD.Load<Texture2D>("res://assets/estados/regen.png") },
		{ Estado.Energetico, GD.Load<Texture2D>("res://assets/estados/energetic.png") },
		{ Estado.Evasion, GD.Load<Texture2D>("res://assets/estados/evasion.png") },
		{ Estado.Marca_del_cazador, GD.Load<Texture2D>("res://assets/estados/hunters_call.png") },
		{ Estado.Creacion, GD.Load<Texture2D>("res://assets/estados/magic_creation.png") },
		{ Estado.Vanguardia, GD.Load<Texture2D>("res://assets/estados/vanguard.png") },
		// De momento estan todos...
	};
	
	public static Texture2D GetIcono(Estado estado){
		return IconosEstado.TryGetValue(estado, out var icono) ? icono : null;
	}
}
