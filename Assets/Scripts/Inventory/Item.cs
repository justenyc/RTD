using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Item_Data.ItemData data;
    public GameObject modelPrefab;

    public Item(Item_Data.ItemData data, GameObject modelPrefab = null)
    {
        this.data = data;
        this.modelPrefab = modelPrefab;
    }
}
