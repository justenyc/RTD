using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public float potency;
    public float power;
    public float size;

    private Action<GameObject> useAction;
    private Action<GameObject> onCollision;
    
    public Item(string _name, string _description = "", float _potency = 0, float _power = 0, float _size = 0, Action<GameObject> _useAction = null, Action<GameObject> _onCollision = null)
    {
        name = _name;
        description = _description;
        potency = _potency;
        power = _power;
        size = _size;
        useAction = _useAction;
        onCollision = _onCollision;
    }

    public static bool operator ==(Item item1, Item item2)
    {
        return item1.name == item2.name;
    }

    public static bool operator !=(Item item1, Item item2)
    {
        return item1.name == item2.name;
    }

    public override bool Equals(object obj)
    {
        Item other = obj as Item;
        return other.name == this.name;
    }

    public override int GetHashCode()
    {
        if (name == null) { return 0; }
        return name.GetHashCode();
    }

    public static Item Nectar
    {
        get
        {
            return new Item(
                _name: "Nectar",
                _description: "A thick, red liquid said to contain the very essence of life itself. Heals a bit of health.",
                _potency: 20,
                _power: 2
                );
        }
    }

    public static Item Soul
    {
        get
        {
            return new Item(
                _name: "Soul of a Lost One",
                _description: "A crystalized essence said to contain the soul of one who couldn't reach a better (brighter?) place. Restores a bit of light",
                _potency: 20,
                _power: 2
                );
        }
    }

    public static Item ParadiseLost
    {
        get
        {
            return new Item(
                _name: "Paradise Lost",
                _potency: 20,
                _power: 2
                );
        }
    }

    public static Item Torch
    {
        get
        {
            return new Item(
                _name: "Torch",
                _description: "A simple device that creates a temporary light source to protect from the dark.",
                _potency: 2,
                _power: 2,
                _size: 10
                );
        }
    }
    public static Item Firebomb
    {
        get
        {
            return new Item(
                _name: "Firebomb",
                _description: "A compound that detonates on impact, leaving a field of flames behind. Crude, but useful.",
                _potency: 5,
                _power: 3,
                _size: 5
                );
        }
    }
}
