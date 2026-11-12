using UnityEngine;

public static class ComponentExtensions
{
    public static T GetComponentInAll<T>(this Component component)
    {
        T found = component.GetComponent<T>() ?? component.GetComponentInChildren<T>() ?? component.GetComponentInParent<T>() ?? component.transform.parent.GetComponent<T>() ?? component.transform.parent.GetComponentInChildren<T>();

        return found;
    }
}