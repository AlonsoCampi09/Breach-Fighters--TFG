using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ObjectType {LIFE_POTION}

public abstract partial class Object: Resource 
{
    [Export] public ObjectType entityType { get; protected set; }

    [Export] public string Name { get; set; } = "";


    public abstract bool Execute(Fighter caster, Fighter target, BattleManager battle);
}