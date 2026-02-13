using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent<Hurtbox> OnHitEvent;
    public List<Collider> collisionExceptions;

    [System.Serializable]
    public enum DamageType
    {
        Slash,
        Blunt,
        Fire,
        Light,
        Dark,
        Explosive,
        InstantDeath
    }

    [System.Serializable]
    public class Args
    {
        public string name = "";
        public float power = 0;
        public float knockback = 0;
        public Vector3 position = Vector3.zero;
        public float size = 0;
        public Vector3 rotation = Vector3.zero;
        public DamageType damageType = DamageType.Blunt;
        public List<GameObject> exceptions;
    }

    public class ArgsBuilder
    {
        Args args = new Args();

        public ArgsBuilder WithName(string _name)
        {
            args.name = _name;
            return this;
        }

        public ArgsBuilder WithPower(float _power)
        {
            args.power = _power;
            return this;
        }

        public ArgsBuilder WithKnockback(float _knockback)
        {
            args.knockback = _knockback;
            return this;
        }

        public ArgsBuilder WithPosition(Vector3 _position)
        {
            args.position = _position;
            return this;
        }

        public ArgsBuilder WithSize(float _size)
        {
            args.size = _size;
            return this;
        }

        public ArgsBuilder WithRotation(Vector3 _rotation)
        {
            args.rotation = _rotation;
            return this;
        }

        public ArgsBuilder WithDamageType(DamageType _damageType)
        {
            args.damageType = _damageType;
            return this;
        }

        public ArgsBuilder WithExceptions(List<GameObject> _exceptions)
        {
            args.exceptions = _exceptions;
            return this;
        }

        public Args Build()
        {
            return args;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < collisionExceptions.Count; i++)
        {
            if (other.gameObject == collisionExceptions[i])
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Hurtbox hurtbox))
        {
            Debug.Log($"{this.name} : {other.gameObject.name}");
            if (OnHitEvent != null)
            {
                OnHitEvent.Invoke(hurtbox);
            }
        }
    }

    public void ProcessHurtbox(Action<Hurtbox> process, Hurtbox hurtbox)
    {
        process?.Invoke(hurtbox);
    }
}
