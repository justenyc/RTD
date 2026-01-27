using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Runtime.InteropServices.WindowsRuntime;

[CreateAssetMenu(fileName = "StatProfile", menuName = "ScriptableObjects/StatProfile")]
public class BaseStatProfile : ScriptableObject
{
    [Tooltip("The base amount of HP the character has at level 1")]
    [SerializeField] float baseHp = 100;

    [Tooltip("Base damage reduction for physical damage")]
    [SerializeField] float physDef = 1;

    [Tooltip("Base damage reduction for other damage")]
    [SerializeField] float elemDef = 1;

    [Tooltip("Base attack rating for physical damage")]
    [SerializeField] float physAtk = 1;

    [Tooltip("Base attack rating for other damage")]
    [SerializeField] float elemAtk = 1;

    [Tooltip("Affects HP scaling per level")]
    [SerializeField] float constitution = 1;

    [Tooltip("Affects physical damage reduction scaling per level")]
    [SerializeField] float toughness = 1;

    [Tooltip("Affects other damage reduction scaling per level")]
    [SerializeField] float spirit = 1;

    [Tooltip("Affects physical damage scaling per level")]
    [SerializeField] float strength = 1;

    [Tooltip("Affect other damage scaling per level")]
    [SerializeField] float mysticism = 1;

    [Tooltip("Specified damage type resistances")]
    [SerializeField] SerializableDictionaryBase<Hitbox.DamageType, float> resistances = new();
    
    public float BaseHp => baseHp;
    public float PhysDef => physDef;
    public float ElemDef => elemDef;
    public float PhysAtk => physAtk;
    public float ElemAtk => elemAtk;
    public float Constitution => constitution;
    public float Toughness => toughness;
    public float Spirit => spirit;
    public float Strength => strength;
    public float Mysticism => mysticism;
    public SerializableDictionaryBase<Hitbox.DamageType, float> Resistances => resistances;
}
