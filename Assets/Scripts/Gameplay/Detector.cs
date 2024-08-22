using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public System.Action<Collider> OnTriggerEnterPost;
    public System.Action<Collider> OnTriggerExitPost;
    public System.Action<Collider> OnTriggerStayPost;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if(OnTriggerEnterPost != null)
        {
            OnTriggerEnterPost(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(OnTriggerExitPost != null)
        {
            OnTriggerExitPost(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if( OnTriggerStayPost != null)
        {
            OnTriggerStayPost(other);
        }
    }
}
