using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent<Hurtbox> OnHitEvent;
    public GameObject[] exceptions;

    [System.Serializable]
    public class Args
    {
        public string attackName;
        public float power;
        public float knockback;
        public Args(float _power, float _knockback, string _attackName = "")
        {
            attackName = _attackName;
            power = _power;
            knockback = _knockback;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(exceptions.Length < 1)
        {
            return;
        }

        for (int i = 0; i < exceptions.Length; i++)
        {
            if (other.gameObject == exceptions[i])
            {
                return;
            }
        }

        if (other.TryGetComponent(out Hurtbox hurtbox))
        {
            Debug.Log($"{this.name} : {other.name}");
            if (OnHitEvent != null)
            {
                OnHitEvent.Invoke(hurtbox);
            }
        }
    }
}
