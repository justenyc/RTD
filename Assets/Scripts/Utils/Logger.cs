using System.Diagnostics;
using UnityEngine;

public static class Logger
{
    [Conditional("UNITY_EDITOR")]
    public static void LogMessage(string message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(string message)
    {
        UnityEngine.Debug.LogError(message);
    }
}
