using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //[SerializeField] SerializableDictionaryBase<Item, int> inventory;
    [SerializeField] DB_Item ItemDB;
    [SerializeField] int maxInventorySize = 10;
    [SerializeField] List<InventorySlot> inventory = new List<InventorySlot>();
    [SerializeField] string m_currentItem = null;
    public string CurrentItem => m_currentItem;

    [System.Serializable]
    class InventorySlot
    {
        public int inventoryIndex;
        public string itemName;
        public int amount;
        public int max;
        
        public InventorySlot(int index, string itemName, int amount, int max)
        {
            this.inventoryIndex = index;
            this.itemName = itemName;
            this.amount = amount;
            this.max = max;
        }
    }

    private void OnValidate()
    {
        if (ItemDB == null)
        {
            Debug.LogWarning($"<color=cyan>Inventory</color> component on <color=green>{gameObject.name}</color> is missing a reference to an <color=yellow>DB_Item</color>!");
        }
    }

    private void OnEnable()
    {
        if (ItemDB != null)
        {
            AddOrRemoveItemFromInventory(ItemDB.GetItem("Firebomb"), 10);
        }
    }

    private void Start()
    {
        if (inventory.Count > 0)
        {
            m_currentItem = inventory[0].itemName;
        }
    }

    InventorySlot IsInInventory(Item item)
    {
        foreach(InventorySlot slot in inventory)
        {
            if(slot.itemName == item.itemName)
            {
                return slot;
            }
        }
        Debug.Log($"Item <color=cyan>{item.itemName}</color> was not found in the Inventory component of <color=green>{gameObject.name}</color>");
        return null;
    }

    public int AddOrRemoveItemFromInventory(Item item, int amount = 1)
    {
        var slot = IsInInventory(item);
        if (slot != null && slot.amount < slot.max)
        {
            inventory[slot.inventoryIndex].amount += amount;

            if (inventory[slot.inventoryIndex].amount < 1)
            {
                if (item.OnUseSuccess != null)
                {
                    foreach (System.Delegate d in item.OnUseSuccess.GetInvocationList())
                    {
                        item.OnUseSuccess -= (System.Action<GameObject>)d;
                    }
                }

                inventory.Remove(slot);
            }
            return 1;
        }

        if(inventory.Count >= maxInventorySize)
        {
            Debug.Log($"<color=cyan>Inventory</color> on <color=green>{gameObject.name}</color> exceeded max number of slots");
            return 2;
        }

        inventory.Add(new InventorySlot(inventory.Count, item.itemName, amount, item.maxStacks));
        return 0;
    }

    public Item GetCurrentItem()
    {
        return ItemDB.GetItem(CurrentItem);
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

    public void UseCurrentItem(GameObject requester)
    {
        UseItem(requester, GetCurrentItem());
    }

    public void UseItem(GameObject requester, Item item)
    {
        if (IsInInventory(item) == null)
        {
            Debug.Log($"<color=cyan>{item.itemName}</color> was not found when attempting <color=yellow>UseItem()</color>");
            return;
        }

        item.Use(this.gameObject, (e) => AddOrRemoveItemFromInventory(item, -item.consumptionRate));
    }
}
