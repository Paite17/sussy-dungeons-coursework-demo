using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    HAND,
    HEAD,
    TORSO,
    LEGS
}

public class Equipment : Item
{
    public EquipType equipType;

    public int strModifier;
    public int defModifier;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        Inventory.instance.RemoveItem(this);
    }

}
