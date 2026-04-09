using System.Transactions;
using UnityEngine;

public class Item_Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider colliderRef;

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

    public void InitProtocol(Item _item, GameObject _thrower = null, Collider[] _exceptions = null)
    {
        item = _item;
        thrower = _thrower;
        exceptions = _exceptions;
        colliderRef.gameObject.transform.localScale *= _item.size;

        args = new Hitbox.ArgsBuilder()
            .WithPower(_item.potency)
            .WithDamageType(_item.damageType)
            .Build();

        return;
    }

    //Set in Inspector
    public void ProcessHurtbox(Hurtbox hurtbox)
    {
        if (args == null)
        {
            Debug.LogError($"<color=cyan>{gameObject.name}</color> Hitbox.Args is <color=red>null</color>");
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
        Debug.Log(hurtbox.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == thrower)
        {
            return;
        }

        foreach (var col in exceptions)
        {
            if (collision.collider == col)
            {
                return;
            }
        }

        if (item.onCollisionVfx != null)
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
