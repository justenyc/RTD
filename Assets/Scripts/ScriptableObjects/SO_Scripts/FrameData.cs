using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FrameData", menuName = "Scriptable Objects/FrameData")]
public class FrameData : ScriptableObject
{
    [SerializeField] SerializableDictionaryBase<FrameDataAddresses, int> frameDataDict = new();

    public SerializableDictionaryBase<FrameDataAddresses, int> FrameDataDict => frameDataDict;
}

public enum FrameDataAddresses
{
    THEA_AIM_MOVESTARTUP,
    THEA_AIM_THROWSTARTUP,
    THEA_AIM_THROWCOOLDOWN,
    THEA_AIM_SWORDAPPEAR,
    THEA_AIM_SWORDDISAPPEAR
}