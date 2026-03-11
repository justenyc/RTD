using UnityEngine;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;

public class DB_SO : MonoBehaviour
{
    public static DB_SO instance;

    public Status_Effects statusEffectsSO;
    public DB_VFX vfxSO;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }

        Destroy(this);
    }
}
