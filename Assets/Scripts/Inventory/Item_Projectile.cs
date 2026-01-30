using System.Transactions;
using UnityEngine;

public class Item_Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Collider colliderRef;

    GameObject thrower;
    Item item;
    Hitbox.Args args;

    public void InitProtocol(Item _item, GameObject _thrower = null)
    {
        item = _item;
        thrower = _thrower;
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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == thrower)
        {
            return;
        }    
        Destroy(gameObject);
    }
}
