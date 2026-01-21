using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DB_Items", menuName = "ScriptableObjects/DB/Items")]
public class DB_Item : ScriptableObject
{
    [SerializeField] List<Item> Items = new();

    public Item GetItem(string name)
    {
        foreach(var item in Items)
        {
            if(item.itemName == name)
            {
                return item;
            }
        }

        Debug.LogError($"No item with name [{name}]");
        return null;
    }

    public Item GetItem(int index)
    {
        if(index  < 0 || index >= Items.Count) 
        {
            Debug.LogError($"Invalid DB_Item index [{index}]");
            return null; 
        }

        return Items[index];
    }
}
