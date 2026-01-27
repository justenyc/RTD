using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public static class Item_Effects
{
    public delegate void OnUseDelegate(GameObject go, Item item, Action<bool> callback);

    public static Dictionary<OnUseEffect, OnUseDelegate> onUseEffects = new Dictionary<OnUseEffect, OnUseDelegate>
    {
        { OnUseEffect.Heal_Health, Heal_Health }
    };

    public static Dictionary<OnCollisionEffect, UnityAction<Collision>> onCollisionEffects = new Dictionary<OnCollisionEffect, UnityAction<Collision>>
    {
        { OnCollisionEffect.AOE_Fire, AOE_Fire }
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
        AOE_Light
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

    public static void AOE_Fire(Collision collision)
    {
        Debug.Log("Collision Happened!");
    }
}