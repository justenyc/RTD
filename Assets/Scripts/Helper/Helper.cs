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

    public static IEnumerator DelayActionByFixedTime(System.Action action, float duration)
    {
        float time = 0;
        while(time < duration)
        {
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        action();
    }

    public static IEnumerator DelayActionByFixedTimeFrames(System.Action action, int frames)
    {
        int currentFrames = 0;
        while (currentFrames < frames)
        {
            currentFrames++;
            Debug.Log($"{currentFrames} : {frames}");
            yield return new WaitForFixedUpdate();
        }
        action();
    }
}
