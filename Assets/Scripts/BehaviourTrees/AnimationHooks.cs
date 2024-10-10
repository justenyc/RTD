using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationHooks : MonoBehaviour
{
    public Action AttackFinishedPost;

    public void AttackFinished()
    {
        Debug.Log("Attack Finished!");
        if(AttackFinishedPost != null)
        {
            AttackFinishedPost();
        }
    }
}
