using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] UnityEvent<Hitbox.Args> OnHurt;
    [SerializeField] UnityEvent<Hitbox.DamageType> OnDamageTypeEffects; 

    /// <summary>
    /// The method to call when Hitbox.Args is ready to be delivered to the receiving Hurtbox
    /// </summary>
    /// <param name="hitboxArgs">The Hitbox.Args in question</param>
    public void PostOnHurt(Hitbox.Args hitboxArgs)
    {
        if(OnHurt != null && hitboxArgs != null)
        {
            OnHurt?.Invoke(hitboxArgs);
        }

        OnDamageTypeEffects?.Invoke(hitboxArgs.damageType);
    }
}
