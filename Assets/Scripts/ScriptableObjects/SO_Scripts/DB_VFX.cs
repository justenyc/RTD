using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DB_VFX", menuName = "Scriptable Objects/Database/VFX DB")]
public class DB_VFX : ScriptableObject
{
    public List<GameObject> battleVfxPrefabs;

    public GameObject GetVfxPrefab(string name, List<GameObject> vfxGroup)
    {
        foreach (var vfx in vfxGroup)
        {
            if (vfx.name == name)
            {
                return vfx;
            }
        }
        Debug.LogError($"No vfx was found in group {vfxGroup} with name {name}");
        return null;
    }
}