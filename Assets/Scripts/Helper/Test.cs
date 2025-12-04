using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"State {stateInfo.shortNameHash} finished on layer {layerIndex}");
    }

    /***
     * using UnityEngine;

public class MyStateExitBehaviour : StateMachineBehaviour
{
    // Called when the state is about to exit (e.g., animation finishing and transitioning)
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"Exited state: {stateInfo.shortNameHash} on layer {layerIndex} (normalized time: {stateInfo.normalizedTime})");
        
        // Your custom logic here, e.g.:
        // HandleAnimationFinished();
    }

    // Optional: For debugging entry
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"Entered state: {stateInfo.shortNameHash}");
    }

    // Optional: Check progress every frame
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.95f) // Near end
        {
            Debug.Log("Animation nearing completion");
        }
    }
}***/
}
