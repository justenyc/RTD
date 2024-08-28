/*
 * Notes:
 * Item_Data and Item are split because a Scriptable object cannot define functions.
 * Therefore, in order to have items predefine their own functions and hold a reference
 * to their model prefab, a amalgamation class was needed. 
 * 
 * Might be able to abstract further and implement a Strategy Pattern depending on how the
 * OnUse() and OnCollision() actions end up looking
*/
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Item_Data
{
    public string itemName;
    [TextArea(3, 10)]
    public string description;
    public float potency;
    public float power;
    public float size;

    public Func<GameObject, bool> useAction;
    public Action<GameObject> onCollision;

    public enum ItemData
    {
        Nectar,
        Soul,
        Torch,
        Firebomb,
        ParadiseLost
    }

    public Item_Data(string _name, string _description = "", float _potency = 0, float _power = 0, float _size = 0, Func<GameObject, bool> _useAction = null, Action<GameObject> _onCollision = null)
    {
        itemName = _name;
        description = _description;
        potency = _potency;
        power = _power;
        size = _size;
        useAction = _useAction;
        onCollision = _onCollision;
    }

    #region Operator Override

    public static bool operator ==(Item_Data item1, Item_Data item2)
    {
        return item1.itemName == item2.itemName;
    }

    public static bool operator !=(Item_Data item1, Item_Data item2)
    {
        return item1.itemName == item2.itemName;
    }

    public override bool Equals(object obj)
    {
        Item_Data other = obj as Item_Data;
        return other.itemName == this.itemName;
    }

    public override int GetHashCode()
    {
        if (itemName == null) { return 0; }
        return itemName.GetHashCode();
    }

    #endregion

    #region Item Definitions

    public static Item_Data GetItemData(ItemData data)
    {
        switch (data)
        {
            case ItemData.Nectar: return Nectar;
            case ItemData.Soul: return Soul;
            case ItemData.Torch: return Torch;
            case ItemData.Firebomb: return Firebomb;
            case ItemData.ParadiseLost: return ParadiseLost;
            default: return null;
        }
    }

    public static Item_Data Nectar
    {
        get
        {
            var nectar = new Item_Data(
                _name: "Nectar",
                _description: "A thick, red liquid said to contain the very essence of life itself. Heals a bit of health.",
                _potency: 20,
                _power: 2
                );

            nectar.useAction = (e) =>
            {
                var hp = e.GetComponent<Health>();
                if(hp)
                {
                    hp.ChangeCurrentHealth(nectar.potency);
                    return true;
                }
                return false;
            };

            nectar.onCollision = (e) =>
            {
                var hp = e.GetComponent<Health>();
                if (hp)
                {
                    hp.ChangeCurrentHealth(-nectar.power);
                }
            };
            return nectar;
        }
    }
    public static Item_Data Soul
    {
        get
        {
            return new Item_Data(
                _name: "Soul of a Lost One",
                _description: "A crystalized essence said to contain the soul of one who couldn't reach a better (brighter?) place. Restores a bit of light",
                _potency: 20,
                _power: 2
                );
        }
    }
    public static Item_Data ParadiseLost
    {
        get
        {
            return new Item_Data(
                _name: "Paradise Lost",
                _potency: 20,
                _power: 2
                );
        }
    }
    public static Item_Data Torch
    {
        get
        {
            return new Item_Data(
                _name: "Torch",
                _description: "A simple device that creates a temporary light source to protect from the dark.",
                _potency: 2,
                _power: 2,
                _size: 10
                );
        }
    }
    public static Item_Data Firebomb
    {
        get
        {
            return new Item_Data(
                _name: "Firebomb",
                _description: "A compound that detonates on impact, leaving a field of flames behind. Crude, but useful.",
                _potency: 5,
                _power: 3,
                _size: 5
                );
        }
    }

    #endregion
}
