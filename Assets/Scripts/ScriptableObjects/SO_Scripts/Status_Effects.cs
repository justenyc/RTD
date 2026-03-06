using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DB_Status_Effect", menuName = "Scriptable Objects/Database/Status Effect DB")]
public class Status_Effects : ScriptableObject
{
    [SerializeField] SerializableDictionaryBase<StatusEffect, GameObject> effects;

    Dictionary<Hitbox.DamageType, StatusEffect> damageStatusPair = new Dictionary<Hitbox.DamageType, StatusEffect>()
    {
        { Hitbox.DamageType.Fire, StatusEffect.Burn }
    };

    public enum StatusEffect
    {
        Burn,
        Stun
    }

    public GameObject GetEffectPrefab(StatusEffect effect)
    {
        if (effects.TryGetValue(effect, out GameObject obj)) 
        {
            return obj;
        }
        return null;
    }

    public void ApplyEffect(Hitbox.Args args, GameObject target)
    {
        if(damageStatusPair.TryGetValue(args.damageType, out StatusEffect effect))
        {
            ApplyEffect(effect, target);
        }
    }

    public void ApplyEffect(StatusEffect effect, GameObject target)
    {
        if(effects.TryGetValue(effect, out GameObject obj))
        {
            var effectPrefab = Instantiate(obj, target.transform.position, Quaternion.identity, target.transform);
            IStatusEffect effectComponent = effectPrefab.GetComponent<IStatusEffect>();
            effectComponent.Apply(target);
        }
    }

    static bool ApplicationCheck(float check)
    {
        check = Mathf.Clamp01(check);
        float chance = Random.Range(0.01f, 0.99f);
        Debug.Log($"ApplicationCheck evaluated to {chance}(chance) against {check}(check)");
        return check > chance;
    }
}