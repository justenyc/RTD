using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] UnityEvent<Hitbox.Args> OnHurt;
    [SerializeField] UnityEvent<Hitbox.DamageType> OnDamageTypeEffects; 

    public void PostOnHurt(Hitbox.Args hitboxArgs)
    {
        if(OnHurt != null && hitboxArgs != null)
        {
            OnHurt.Invoke(hitboxArgs);
        }

        OnDamageTypeEffects?.Invoke(hitboxArgs.damageType);
    }

    //Set in Inspector
    public void Burn(Hitbox.DamageType damageType)
    {
        if(damageType == Hitbox.DamageType.Fire)
        {
            Debug.Log($"{gameObject.name} is being <color=orange>BURNED!!!</color>");
        }
    }

    public void Demolish(Hitbox.DamageType damageType)
    {
        if(damageType == Hitbox.DamageType.Explosive)
        {
            Debug.Log($"{gameObject.name} is being <color=red>DEMOLISHED!!!</color>");
            Destroy(gameObject);
        }
    }
}
