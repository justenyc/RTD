using System.Transactions;
using UnityEngine;

public class Item_Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider colliderRef;

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
        Item_Effects.onCollisionEffects[item.onCollisionEffect].Invoke(this.gameObject, item, null);
        Debug.Log(hurtbox.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == thrower)
        {
            return;
        }

        foreach(var col in exceptions)
        {
            if(other == col)
            {
                return;
            }
        }
        Destroy(gameObject);
    }
}
