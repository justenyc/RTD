using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hurtbox))]
public class Destructable : MonoBehaviour
{
    [SerializeField] float burnTime = 3f;
    delegate void DestructableDelegate(GameObject affected);

    Dictionary<Hitbox.DamageType, DestructableDelegate> dict = new Dictionary<Hitbox.DamageType, DestructableDelegate>()
    {
        { Hitbox.DamageType.Fire, Burn },
        { Hitbox.DamageType.Explosive, Demolish }
    };

    public void OverrideBurnTime(ref float newTime)
    {
        newTime = burnTime;
    }

    public void ProcessHurtbox(Hitbox.Args args)
    {
        if(dict.TryGetValue(args.damageType, out var outDelegate))
        {
            outDelegate.Invoke(this.gameObject);
        }
    }

    static void Burn(GameObject affected)
    {
        DB_SO.instance.statusEffectsSO.ApplyEffect(Status_Effects.StatusEffect.Burn, affected);
    }

    static void Demolish(GameObject affected)
    {
        Destroy(affected);
    }
}