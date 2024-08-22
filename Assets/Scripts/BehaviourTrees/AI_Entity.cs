using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Entity : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent m_agent;

    protected BehaviourTree m_behaviourTree = new BehaviourTree("baseTree");

    [SerializeField] string treeStatus;
    
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        treeStatus = m_behaviourTree.Process().ToString();
    }

    protected List<Vector3> ParsePatrolPoints(List<Transform> points)
    {
        List<Vector3> listToReturn = new();

        foreach(Transform t in points)
        {
            listToReturn.Add(t.position);
        }

        return listToReturn;
    }
}
