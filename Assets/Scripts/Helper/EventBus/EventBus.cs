using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : MonoBehaviour
{
    
}

[System.Serializable]
public struct CustomEventArgs
{
    public string name;
    public GameObject origin;

    public string stringArg;
    public bool boolArg;
    public float floatArg;
}
