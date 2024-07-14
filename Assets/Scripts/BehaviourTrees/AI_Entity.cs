using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Entity : MonoBehaviour
{
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] Transform m_patrolPointContainer;
    [SerializeField] List<Transform> m_patrolPoints;
    [SerializeField] bool detectedSomething;

    BehaviourTree m_behaviourTree = new BehaviourTree("baseTree");
    
    private void Start()
    {

        m_behaviourTree.AddChild(new Leaf("Patrol", new IStrategy.PatrolStrategy(this.transform, m_agent, m_patrolPoints)));
    }

    private void Update()
    {
        m_behaviourTree.Process();
    }

    IEnumerator DelayAction(float time)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
        }
        m_behaviourTree.children[0].ForceComplete();
    }
}
