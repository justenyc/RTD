using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Blackboard 
{
    Dictionary<string, object> entries = new();

    public Blackboard() { }

    public object GetEntryByKey<T>(string key)
    { 
        if(!entries.ContainsKey(key))
        {
            return default;
        }
        var entry = entries[key];
        return entry;
    }

    public bool AddEntry<T>(string key, T value)
    {
        if(!entries.ContainsKey(key))
        {
            entries[key] = new BlackboardEntry<T>(key, value);
            return true;
        }
        return false;
    }

    public void SetValue<T>(string key, T value)
    {
        if (!AddEntry<T>(key, value))
        {
            entries[key] = new BlackboardEntry<T>(key, value);
        }
    }
}

public class BlackboardEntry<T>
{
    public string name;
    public T value;
    public Type type;

    public BlackboardEntry(string name, T value)
    {
        this.name = name;
        this.value = value;
        this.type = typeof(T);
    }
}
