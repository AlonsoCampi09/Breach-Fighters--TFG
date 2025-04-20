using Godot;
using System;
using System.Linq;
using System.Collections.Generic;  // This is the library that includes Dictionary


public enum Estado
{
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
	Vanguardia
	//etc
}

public partial class EstadoManager
{
	
	private Dictionary<Estado, int> estados = new Dictionary<Estado, int>();

	private Entity myEntity;
	
	public EstadoManager(Entity e){
		myEntity = e;
	}

	// Aplicar o renovar un estado
	public void AplicarEstado(Estado estado, int duracion, int ptg){
		
		if (this.TieneEstado(estado)){
			if(estado == Estado.BuffDMG && myEntity.newATQBuffIsBetter(ptg)){
				myEntity.addBuffDMG(ptg);
				estados[estado] = duracion; //renovar duración
			}else if(estado == Estado.DeBuffDMG && myEntity.newATQDeBuffIsBetter(ptg)){
				myEntity.addDeBuffDMG(ptg);
				estados[estado] = duracion; //renovar duración
			}else if(estado == Estado.BuffDEF && myEntity.newDEFBuffIsBetter(ptg)){
				myEntity.addBuffDEF(ptg);
				estados[estado] = duracion; //renovar duración
			}else if(estado == Estado.DeBuffDEF && myEntity.newDEFDeBuffIsBetter(ptg)){
				myEntity.addDeBuffDEF(ptg);
				estados[estado] = duracion; //renovar duración
			}else
				estados[estado] = duracion; //renovar duración
		}
		else
			estados.Add(estado, duracion);//Insertar
	}

	// Reducir duración y eliminar expirados
	public void AvanzarTurno()
	{
		var expirados = new List<Estado>();

		List<Estado> claves = new List<Estado>(estados.Keys);
		
		foreach (var estado in claves){
			estados[estado]--;
			if (estados[estado] <= 0)
				expirados.Add(estado);
		}
		foreach (var estado in expirados){
			if(estado == Estado.BuffDMG){
				myEntity.removeBuffDMG();
				this.EliminarEstado(estado);
			}else if(estado == Estado.DeBuffDMG){
				myEntity.removeDeBuffDMG();
				this.EliminarEstado(estado);
			}else if(estado == Estado.BuffDEF){
				myEntity.removeBuffDEF();
				this.EliminarEstado(estado);
			}else if(estado == Estado.DeBuffDEF){
				myEntity.removeDeBuffDEF();
				this.EliminarEstado(estado);
			}else if(estado != Estado.Envenenado){
				this.EliminarEstado(estado);
			}
		}
	}

	public bool TieneEstado(Estado estado){
		return estados.ContainsKey(estado);
	}
	
	public int GetDuracion(Estado estado){
		if (this.TieneEstado(estado)){
			return estados[estado];
		}else
			return 0;
	}

	public void EliminarEstado(Estado estado){
		estados.Remove(estado);
	}
	
	public void Reinicio_efecto(){
		bool veneno = false;
		if(TieneEstado(Estado.Envenenado))
			veneno = true;
		estados.Clear();
		if(veneno){
			AplicarEstado(Estado.Envenenado,0,0);
		}
	}
	
	public void Restauracion_efecto(){
		if(TieneEstado(Estado.DeBuffDMG))
			EliminarEstado(Estado.DeBuffDMG);
		if(TieneEstado(Estado.DeBuffDEF))
			EliminarEstado(Estado.DeBuffDEF);
		if(TieneEstado(Estado.DeBuffVEL))
			EliminarEstado(Estado.DeBuffVEL);
		if(TieneEstado(Estado.Aturdido))
			EliminarEstado(Estado.Aturdido);
		if(TieneEstado(Estado.Sellado))
			EliminarEstado(Estado.Sellado);
		if(TieneEstado(Estado.Bloqueo))
			EliminarEstado(Estado.Bloqueo);
	}

	public List<Estado> GetEstados(){
		return estados.Keys.ToList();
	}
	
}
