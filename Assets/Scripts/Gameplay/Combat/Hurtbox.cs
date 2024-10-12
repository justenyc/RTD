using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Action<Hitbox.Args> OnHurt;

    public void PostOnHurt(Hitbox.Args hitboxArgs)
    {
        if(OnHurt != null)
        {
            OnHurt(hitboxArgs);
        }
    }
}
