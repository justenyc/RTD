using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    Blackboard bb = new Blackboard();
    void Start()
    {
        //bb.AddEntry("Entry1", new BlackboardEntry<bool>("Entry1", true));
        //var test = bb.GetEntryByKey<bool>("Entry1") as BlackboardEntry<bool>; 
        //Debug.Log(test.value);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position) * 100, ForceMode.Impulse);
    }
}
