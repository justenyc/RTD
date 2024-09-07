using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFactory", menuName = "Factories/ItemFactory")]
public class ItemFactory : ScriptableObject
{
    [SerializeField] SerializableDictionaryBase<string, Item> Items = new SerializableDictionaryBase<string, Item>();

    public Item GetItemByName(string name)
    {
        if (Items.ContainsKey(name))
        {
            return Items[name];
        }

        Debug.LogError($"No item with name [{name}] found");
        return null;
    }
}
