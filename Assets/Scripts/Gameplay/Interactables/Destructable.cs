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
        { Hitbox.DamageType.Explosive, Demolish },
        { Hitbox.DamageType.Slash, Demolish },
        { Hitbox.DamageType.Blunt, Demolish }
    };

    public void OverrideBurnTime(ref float newTime)
    {
        newTime = burnTime;
    }

    /// <summary>
    /// Hurtbox component delivers what Hitbox.Args was delivered to it and leaves it up to the receiver to process. Easiest to just set in inspector
    /// </summary>
    /// <param name="args">The Hitbox.Args received from the Hurtbox Component</param>
    public void ProcessHurtbox(Hitbox.Args args)
    {
        if(dict.TryGetValue(args.damageType, out var outDelegate))
        {
            outDelegate.Invoke(this.gameObject);
            return;
        }

        Logger.LogError($"A handler for the {args.damageType} Hitbox.DamageType was not found on {gameObject.name}");
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