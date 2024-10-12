using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Action<Hurtbox> OnHit;
    public class Args
    {
        public float power;
        public Args(float _power)
        {
            power = _power;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Hurtbox hurtbox))
        {
            Debug.Log($"{this.name} : {other.name}");
            if (OnHit != null)
            {
                OnHit(hurtbox);
            }
        }
    }
}
