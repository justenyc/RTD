using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent<Hurtbox> OnHitEvent;
    public List<GameObject> exceptions;

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

    //[System.Serializable]
    //public class Args
    //{
    //    public string attackName;
    //    public float power;
    //    public float knockback;
    //    public DamageType damageType;

    //    public Args(float _power, float _knockback, string _attackName = "", DamageType _damageType = default)
    //    {
    //        attackName = _attackName;
    //        power = _power;
    //        knockback = _knockback;
    //        damageType = _damageType;
    //    }
    //}

    [System.Serializable]
    public class Args
    {
        public string name { get; set; } = "";
        public float power { get; set; } = 0;
        public float knockback { get; set; } = 0;
        public Vector3 position { get; set; } = Vector3.zero;
        public float size { get; set; } = 0;
        public Vector3 rotation { get; set; } = Vector3.zero;
        public DamageType damageType { get; set; } = DamageType.Blunt;
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

        public Args Build()
        {
            return args;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < exceptions.Count; i++)
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
