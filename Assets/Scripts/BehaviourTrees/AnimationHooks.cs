using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationHooks : MonoBehaviour
{
    public Action AttackFinishedPost;
    public Action CanCancel;
    public Action<string> EnableObjectPost;
    public Action<string> DisableObjectPost;

    public void AttackFinished()
    {
        //Debug.Log("Attack Finished!");
        if(AttackFinishedPost != null)
        {
            AttackFinishedPost();
        }
    }

    public void OnCanCancel()
    {
        if(CanCancel != null)
        {
            CanCancel();
        }
    }
}
