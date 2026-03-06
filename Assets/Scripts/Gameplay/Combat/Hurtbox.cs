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
            OnHurt?.Invoke(hitboxArgs);
        }

        OnDamageTypeEffects?.Invoke(hitboxArgs.damageType);
    }
}
