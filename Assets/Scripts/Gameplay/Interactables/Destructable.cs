using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hurtbox))]
public class Destructable : MonoBehaviour
{
    [SerializeField] float burnTime = 3f;

    public void OverrideBurnTime(ref float newTime)
    {
        newTime = burnTime;
    }

    public void ProcessHurtbox(Hitbox.Args args)
    {
        switch (args.damageType)
        {
            case Hitbox.DamageType.Fire:
                Burn();
                break;

            case Hitbox.DamageType.Explosive:
                Destroy(this.gameObject);
                break;
        }
    }

    void Burn()
    {
        DB_SO.instance.statusEffectsSO.ApplyEffect(Status_Effects.StatusEffect.Burn, gameObject);
    }

    void Demolish(Hitbox.DamageType damageType)
    {
        if (damageType == Hitbox.DamageType.Explosive)
        {
            Debug.Log($"{gameObject.name} is being <color=red>DEMOLISHED!!!</color>");
            Destroy(gameObject);
        }
    }
}