using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public static class Item_Effects
{
    public delegate void OnUseDelegate(GameObject go, Item item, Action<bool> callback);
    public delegate void OnCollisionDelegate(GameObject go, Item item, Action<bool> callback);

    public static Dictionary<OnUseEffect, OnUseDelegate> onUseEffects = new()
    {
        { OnUseEffect.Heal_Health, Heal_Health }
    };

    public static Dictionary<OnCollisionEffect, OnCollisionDelegate> onCollisionEffects = new()
    {
        { OnCollisionEffect.AOE_Damage, AOE_Damage },
        { OnCollisionEffect.AOE_Fire, AOE_Fire },
        { OnCollisionEffect.AOE_Explosive, AOE_Explode }
    };

    public enum OnUseEffect
    {
        None,
        Heal_Health,
        Heal_Light,
        Torch_Ignite
    }

    public enum OnCollisionEffect
    {
        None,
        AOE_Fire,
        AOE_Light,
        AOE_Damage,
        AOE_Explosive
    }

    public static void Heal_Health(GameObject go, Item item, Action<bool> callback = null)
    {
        Status status = go.GetComponent<Status>();
        if (!go)
        {
            Debug.LogError($"Did not find a Health component on {go.name}");
            callback(false);
            return;
        }

        status.ChangeCurrentHealth(item.potency);
        callback(true);
    }

    public static void AOE_Fire(GameObject go, Item item, Action<bool> callback = null)
    {
        // CONSIDER: Maybe scale hitbox args with Status?
        Collider[] overlaps = Physics.OverlapBox(go.transform.position, go.transform.lossyScale * item.size / 2);
        Hitbox.Args hitboxArgs = new Hitbox.ArgsBuilder()
            .WithPower(1f)
            .WithDamageType(Hitbox.DamageType.Fire)
            .Build();

        foreach (Collider collider in overlaps)
        {
            collider.GetComponent<Hurtbox>()?.PostOnHurt(hitboxArgs);
        }
    }

    public static void AOE_Explode(GameObject go, Item item, Action<bool> callback = null)
    {
        Collider[] overlaps = Physics.OverlapBox(go.transform.position, go.transform.lossyScale * item.size / 2);
        Hitbox.Args hitboxArgs = new Hitbox.ArgsBuilder()
            .WithPower(1f)
            .WithDamageType(Hitbox.DamageType.Explosive)
            .Build();

        foreach (Collider collider in overlaps)
        {
            collider.GetComponent<Hurtbox>()?.PostOnHurt(hitboxArgs);
        }

        //Refactor to "OnCollisionVFX"
        var vfxSO = DB_SO.instance.vfxSO;
        var prefab = vfxSO.GetVfxPrefab("VFX_Explosion_Small", vfxSO.battleVfxPrefabs);

        MonoBehaviour.Instantiate(prefab, go.transform.position, Quaternion.identity);
    }

    public static void AOE_Damage(GameObject go, Item item, Action<bool> callback = null)
    {
        Collider[] overlaps = Physics.OverlapBox(go.transform.position, go.transform.lossyScale * item.size / 2);

        Hitbox.Args args = new Hitbox.ArgsBuilder()
                    .WithPower(item.potency)
                    .WithSize(item.size)
                    .WithDamageType(item.damageType)
                    .Build();

        Debug.Log($"<color=yellow>AOE_Damage</color> overlaps length: {overlaps.Length}");
        foreach (Collider c in overlaps)
        {
            if (c.TryGetComponent(out Hurtbox hurtbox))
            {
                hurtbox.PostOnHurt(args);
            }

        }
    }
}