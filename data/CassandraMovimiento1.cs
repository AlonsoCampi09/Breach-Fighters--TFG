using Godot;
using System;

public partial class CassandraMovimiento1 : Moviento{
	
	
	public CassandraMovimiento1(){
		this.effectObj = Effect_Obj.Enemy;
	}
	
	public override void efecto(){
		//Logica del movimiento;
		GD.Print("Cassandra usa Brecha Umbría o Ruptura!");
	}
	
	public override string giveTitulo(int level){
		if(level < 4){
			return "Brecha umbría";
		}else{
			return "Ruptura";
		}
	}
	public override string giveDescripcion(int level){
		if(level < 4){
			return "Ataque especial que ataca a dos enemigos haciendo daño moderado.";
		}else{
			return "Movimiento especial que ataca de todos enemigos haciendo daño moderado. ";
		}
	}
	
}
