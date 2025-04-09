using Godot;
using System;

[GlobalClass]
public partial class CassandraData : Entity
{
	
	public CassandraData(int level){
		Level = level;
        ID = ID_generator;
        ID_generator++;
        Name = "Cassandra";
		
		ControlPlayer = true;
		Turn = false;
		
		TrueHealth = new int[] {108,20,22,24,26,28,30,32,34,36};
		TrueAttack = new int[] {70,9,11,13,15,17,19,21,23,25};
		TrueDefense = new int[] {4,5,6,7,8,9,10,11,12,13};
		TrueMana = new int[] {18,21,24,27,30,33,36,39,42,45};
		TrueSpeed = new int[] {60,7,8,9,10,11,12,13,14,15};
		
		Health = TrueHealth[level-1];
		Mana = TrueMana[level-1];
	}
	
	public override void levelUp(){
		Level++;
		Health += 2;
		Mana += 3;
	}
}
