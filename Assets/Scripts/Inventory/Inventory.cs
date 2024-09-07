using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Inventory : MonoBehaviour
{
    //[SerializeField] SerializableDictionaryBase<Item, int> inventory;
    [SerializeField] ItemFactory itemFactory;
    [SerializeField] List<InventorySlot> inventory = new List<InventorySlot>();
    [SerializeField] Item m_currentItem;
    public Item currentItem => m_currentItem;

    [System.Serializable]
    class InventorySlot
    {
        public int index;
        public Item item;
        public int amount;
        
        public InventorySlot(int index, Item item, int amount)
        {
            this.index = index;
            this.item = item;
            this.amount = amount;
        }
    }

    private void Start()
    {
        inventory.Add(new InventorySlot(inventory.Count, itemFactory.GetItemByName("Nectar"), 10));
        inventory.Add(new InventorySlot(inventory.Count, itemFactory.GetItemByName("Soul"), 3));
        inventory.Add(new InventorySlot(inventory.Count, itemFactory.GetItemByName("Torch"), 20));
        inventory.Add(new InventorySlot(inventory.Count, itemFactory.GetItemByName("Firebomb"), 5));
        inventory.Add(new InventorySlot(inventory.Count, itemFactory.GetItemByName("ParadiseLost"), 2));

        m_currentItem = inventory[0].item;
    }

    InventorySlot IsInInventory(Item item)
    {
        foreach(InventorySlot slot in inventory)
        {
            if(slot.item == item)
            {
                return slot;
            }
        }
        return null;
    }

    public void AddOrRemoveItemFromInventory(Item item, int amount = 1)
    {
        var slot = IsInInventory(item);
        if (slot != null)
        {
            inventory[slot.index].amount += amount;

            if (inventory[slot.index].amount < 1)
            {
                inventory.Remove(slot);
            }
            return;
        }

        inventory.Add(new InventorySlot(inventory.Count, item, 1));
    }

    public Item RequestItem(Item item)
    {
        var slot = IsInInventory(item);
        if (slot != null)
        {
            AddOrRemoveItemFromInventory(item, -1);
            return item;
        }
        return null;
    }
}
