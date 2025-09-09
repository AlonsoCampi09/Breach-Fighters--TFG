using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[GlobalClass]
public partial class LifePotion: Object
{

    public override bool Execute(Fighter caster, Fighter target, BattleManager battle)
    {
        throw new NotImplementedException();
    }

    public override string GetShopDescription()
    {
        return this.Name + "x" + this.ShopStock + "  " + this.Price; 
    }
}