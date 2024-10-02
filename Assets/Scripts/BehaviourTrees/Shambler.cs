using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Shambler : AI_Entity
{
    [Header("Properties")]
    [SerializeField] float m_moveSpeed = 2;
    [SerializeField] float m_turnSpeed = 1;
    [SerializeField] LayerMask m_layerMask;

    [Header("References")]
    [SerializeField] Transform m_mainTransform;
    [SerializeField] List<Transform> m_patrolPoints;
    [SerializeField] Detector m_detector;
    [SerializeField] Animator m_animator;

    Blackboard m_blackboard = new();

    #region Public Accessors

    public Animator Animator => m_animator;

    #endregion

    protected override void Start()
    {
        m_blackboard.AddEntry<bool>("detectedSomething", false);

        m_detector.OnTriggerStayPost += Search;
        m_detector.OnTriggerEnterPost += OnDetect;
        m_detector.OnTriggerExitPost += SearchLost;

        var patrolPoints = base.ParsePatrolPoints(m_patrolPoints);

        var shamblerAiSelector = new PrioritySelector("ShamblerAiSelector");

        var seq = new Sequence("DetectedSomething", 100);
        seq.AddChild(new Leaf("Condition", new IStrategy.Condition(() =>
        {
            var detection = m_blackboard.GetEntryByKey<bool>("detectedSomething") as BlackboardEntry<bool>;
            return detection.value;
        })));
        seq.AddChild(new Leaf("GoToZero", new IStrategy.ActionStrategy(() => m_agent.SetDestination(Vector3.zero))));

        shamblerAiSelector.AddChild(seq);
        shamblerAiSelector.AddChild(new Leaf("MoveBetweenPoints", new IStrategy.MoveBetweenPoints(base.m_agent, patrolPoints, m_moveSpeed)));

        base.m_behaviourTree.AddChild(shamblerAiSelector);
    }

    protected override void Update()
    {
        var lookVector = m_agent.steeringTarget - m_mainTransform.position;
        m_mainTransform.rotation = Quaternion.LookRotation(lookVector.normalized);
        m_animator.SetFloat("Movement", 0.5f);
        base.Update();
    }

    protected virtual void OnDetect(Collider other)
    {
        m_blackboard.AddEntry<bool>("detectedSomething", false);
    }

    protected virtual void Search(Collider other)
    {
        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, other.transform.position - this.transform.position, out hit, 100, m_layerMask))
        {
            Debug.DrawRay(this.transform.position, other.transform.position - this.transform.position, Color.cyan);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_blackboard.SetValue("detectedSomething", true);
                m_blackboard.SetValue("detectedCollider", other);
                //Debug.Log(other.transform.position);
            }
        }
    }

    protected virtual void SearchLost(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            System.Action act = () =>
            {
                m_blackboard.SetValue("detectedSomething", false);
            };

            var searchCollider = m_blackboard.GetEntryByKey<Collider>("detectedCollider") as BlackboardEntry<Collider>;
            Debug.Log(searchCollider.value.transform.position);
            searchCollider.value = null;
            this.StopAllCoroutines();
            StartCoroutine(Helper.DelayActionByTime(act, 5));
        }
    }
}