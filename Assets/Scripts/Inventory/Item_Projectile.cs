using System;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider colliderRef;
    [SerializeField] GameObject vfx;

    [Header("Settings")]
    [SerializeField] int lifeTime = 300;
    [SerializeField] bool destroyOnCollsion = true;

    GameObject thrower;
    Item item;
    Hitbox.Args args;
    Collider[] exceptions;

    public void SetExceptions(Collider[] cols)
    {
        exceptions = cols;
    }

    public void InitProtocol(Item _item, GameObject _thrower = null, Collider[] _exceptions = null, bool vfxOnStart = true, Action callback = null)
    {
        item = _item;
        thrower = _thrower;
        exceptions = _exceptions;
        colliderRef.gameObject.transform.localScale *= _item.size;
        vfx.SetActive(vfxOnStart);

        args = new Hitbox.ArgsBuilder()
            .WithPower(_item.potency)
            .WithDamageType(_item.damageType)
            .Build();

        callback?.Invoke();

        return;
    }

    //Set in Inspector
    public void ProcessHurtbox(Hurtbox hurtbox)
    {
        if (args == null)
        {
            Logger.LogError($"<color=cyan>{gameObject.name}</color> Hitbox.Args is <color=red>null</color>");
            return;
        }
        hurtbox.PostOnHurt(args);
    }

    //Set in Inspector
    public void OnCollisionEffect(Hurtbox hurtbox)
    {
        if(item.onCollisionEffects.Length < 1)
        {
            return;
        }

        foreach (var collisionEffect in item.onCollisionEffects)
        {
            Item_Effects.onCollisionEffects[collisionEffect].Invoke(this.gameObject, item, null);
        }
        Logger.LogMessage(hurtbox.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == thrower)
        {
            return;
        }

        if (!exceptions.IsUnityNull())
        {
            foreach (var col in exceptions)
            {
                if (collision.collider == col)
                {
                    return;
                }
            }
        }

        if (!item.onCollisionVfx.IsUnityNull())
        {
            Instantiate(item.onCollisionVfx, transform.position, Quaternion.identity);
        }

        if(destroyOnCollsion)
        {
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject, lifeTime);
    }
}
