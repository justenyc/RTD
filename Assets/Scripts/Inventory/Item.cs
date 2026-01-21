using System;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    [TextArea(3, 10)]
    public string description;
    public float potency;
    public float size = 1;
    public int consumptionRate = 1;
    public int maxStacks = 99;
    public GameObject modelPrefab;

    public Item_Effects.OnUseEffect onUseEffect;
    public Item_Effects.OnCollisionEffect onCollisionEffect;

    public Action<GameObject> OnUse;

    public void Use(GameObject go, Item item)
    {
        Item_Effects.onUseEffects[onUseEffect].Invoke(go, item);

        if(OnUse != null)
        {
            OnUse(go);
        }
    }

    public void Throw(RigidbodyThrower thrower, Vector3 direction, float throwStrength, int framesToDelayThrow = 0)
    {
        var rb = modelPrefab?.GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogError($"A <color=cyan>Rigidbody</color> was not found on the Model Prefab: <color=yellow>{modelPrefab.name}</color>");
            return;
        }

        thrower.OverrideCollisionEnter(Item_Effects.onCollisionEffects[onCollisionEffect]);

        thrower.StartCoroutine(Helper.DelayActionByFixedTimeFrames(() => thrower.ThrowGameObject(modelPrefab, direction.normalized * throwStrength + Vector3.up * throwStrength * 0.1f), framesToDelayThrow)); 
    }
}
