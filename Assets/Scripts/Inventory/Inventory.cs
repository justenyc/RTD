using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Inventory : MonoBehaviour
{
    [SerializeField] SerializableDictionaryBase<Item, int> inventory;

    public void AddOrRemoveItemFromInventory(Item item, int amount)
    {
        if(inventory.ContainsKey(item))
        {
            inventory[item] += amount;

            if(inventory[item] < 1)
            {
                inventory.Remove(item);
            }
            return;
        }

        inventory.Add(item, 1);
    }
}
