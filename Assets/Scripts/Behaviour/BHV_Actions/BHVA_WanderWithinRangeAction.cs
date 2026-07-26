using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BHVA_Wander Within Range", story: "[Agent] wanders randomly within [Bounds]", category: "Action", id: "9ec17d9c616a75e9538c052cde1a24b2")]
public partial class BHVA_WanderWithinRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<BoxCollider> Bounds;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    [CreateProperty] private float m_OriginalStoppingDistance = -1f;
    [CreateProperty] private float m_OriginalSpeed = -1f;
    private float m_CurrentSpeed;
    private Vector3 randomPositionWithinBounds;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Bounds.Value == null)
        {
            return Status.Failure;
        }

        randomPositionWithinBounds = new Vector3(
            UnityEngine.Random.Range(Bounds.Value.bounds.min.x, Bounds.Value.bounds.max.x),
            UnityEngine.Random.Range(Bounds.Value.bounds.min.y, Bounds.Value.bounds.max.y),
            UnityEngine.Random.Range(Bounds.Value.bounds.min.z, Bounds.Value.bounds.max.z)
            );

        NavMeshHit hit;
        for (int ii = 0; ii < 10; ii++)
        {
            if (NavMesh.SamplePosition(randomPositionWithinBounds, out hit, Bounds.Value.bounds.extents.magnitude, NavMesh.AllAreas))
            {
                randomPositionWithinBounds = hit.position;
                bool result = Agent.Value.SetDestination(randomPositionWithinBounds);
                return Initialize();
            }
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {

        if (Agent.Value == null)
        {
            return Status.Failure;
        }

        Vector3 agentPosition, locationPosition;
        float distance = GetDistanceToLocation(out agentPosition, out locationPosition);
        bool destinationReached = distance <= DistanceThreshold;

        if (destinationReached && (m_NavMeshAgent == null || !m_NavMeshAgent.pathPending))
        {
            return Status.Success;
        }
        //else if (m_NavMeshAgent == null) // transform-based movement
        //{
        //    m_CurrentSpeed = NavigationUtility.SimpleMoveTowardsLocation(Agent.Value.transform, locationPosition, Speed, distance, SlowDownDistance);
        //}

        UpdateAnimatorSpeed();

        return Status.Running;
    }

    private Status Initialize()
    {
        if (GetDistanceToLocation(out Vector3 agentPosition, out Vector3 locationPosition) <= DistanceThreshold)
        {
            return Status.Success;
        }

        // If using a navigation mesh, set target position for navigation mesh agent.
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            //if (m_NavMeshAgent.isOnNavMesh)
            //{
            //    m_NavMeshAgent.ResetPath();
            //}

            m_OriginalSpeed = m_NavMeshAgent.speed;
            m_NavMeshAgent.speed = Speed;
            m_OriginalStoppingDistance = m_NavMeshAgent.stoppingDistance;
            m_NavMeshAgent.stoppingDistance = DistanceThreshold;
            //bool result = m_NavMeshAgent.SetDestination(locationPosition);
            //Debug.Log(result);
        }

        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        UpdateAnimatorSpeed(0f);

        return Status.Running;
    }

    private float GetDistanceToLocation(out Vector3 agentPosition, out Vector3 locationPosition)
    {
        agentPosition = Agent.Value.transform.position;
        locationPosition = randomPositionWithinBounds;
        return Vector3.Distance(new Vector3(agentPosition.x, locationPosition.y, agentPosition.z), locationPosition);
    }

    private void UpdateAnimatorSpeed(float explicitSpeed = -1)
    {
        //NavigationUtility.UpdateAnimatorSpeed(m_Animator, AnimatorSpeedParam, m_NavMeshAgent, m_CurrentSpeed, explicitSpeed: explicitSpeed);
    }

    protected override void OnEnd()
    {
    }
}
