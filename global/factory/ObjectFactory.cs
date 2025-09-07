using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ObjectFactory
{

    public List<Object> GetShopItems ()
    {
        List<Object> items = new List<Object>();
        items.Add(ResourceLoader.Load<Object>("res://data/objects/LifePotion.tres"));
        return items; 
    }


}