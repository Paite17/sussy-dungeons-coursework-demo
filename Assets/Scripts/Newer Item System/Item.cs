using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HEAL,
    OFFENSIVE,
    DEFENSIVE,
    EQUIPMENT
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public ItemType type;
    //public Sprite icon;

    public virtual void Use()
    {
        // this function is meant to be overriden
    }

    public virtual void Drop()
    {
        Inventory.instance.RemoveItem(this);
        // will be overriden if further functionality is needed
    }
}
