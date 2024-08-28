using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFactory", menuName = "Factories/ItemFactory")]
public class ItemFactory : ScriptableObject
{
    [SerializeField] SerializableDictionaryBase<string, Item> Items = new SerializableDictionaryBase<string, Item>();
}
