using UnityEngine;

public class Item_Factory : MonoBehaviour 
{
    [SerializeField] DB_Item itemDb;

    public Item GetItem(int index)
    {
        return itemDb.GetItem(index);
    }

    public Item GetItem(string name)
    {
        return itemDb.GetItem(name);
    }
}