using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] UnityEvent<Hitbox.Args> OnHurt;

    public void PostOnHurt(Hitbox.Args hitboxArgs)
    {
        if(OnHurt != null && hitboxArgs != null)
        {
            OnHurt.Invoke(hitboxArgs);
        }
    }
}
