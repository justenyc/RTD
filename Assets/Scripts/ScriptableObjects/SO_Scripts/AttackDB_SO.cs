using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDB", menuName = "Scriptable Objects/Database/AttackDB")]
public class AttackDB_SO : ScriptableObject
{
    [SerializeField] SerializableDictionaryBase<string, Hitbox.Args> DB = new();

    public Hitbox.Args GetArgsByName(string s)
    {
        if(!DB.ContainsKey(s))
        {
            Debug.LogError($"<color=yellow>{s}</color> was not found in <color=#90EE90>{this.name}</color>");
            return null;
        }
        return DB[s];
    }

    public int GetDbLength()
    {
        return DB.Count;
    }
}