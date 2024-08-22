using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static IEnumerator DelayActionByTime(System.Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
