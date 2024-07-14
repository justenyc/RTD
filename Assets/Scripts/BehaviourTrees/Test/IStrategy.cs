using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IStrategy
{
    Node.Status Process();
    void Reset() { }

    public class Condition : IStrategy
    {
        readonly Func<bool> predicate;

        public Condition(Func<bool> _predicate)
        {
            predicate = _predicate;
        }

        public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
    }

    public class ActionStrategy : IStrategy
    {
        readonly Action action;

        public ActionStrategy(Action _action)
        {
            action = _action;
        }

        public Node.Status Process()
        {
            action();
            return Node.Status.Success;
        }
    }

    public class PatrolStrategy : IStrategy
    {
        Transform entity;
        NavMeshAgent agent;
        List<Transform> patrolPoints;
        float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        public PatrolStrategy(Transform _entity, NavMeshAgent _agent, List<Transform> _patrolPoints, float _patrolSpeed = 2f)
        {
            entity = _entity;
            agent = _agent;
            patrolPoints = _patrolPoints;
            patrolSpeed = _patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count)
            {
                //Reset();
                return Node.Status.Success;
            }

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            //entity.LookAt(target);

            if(isPathCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }

            if(agent.pathPending)
            {
                isPathCalculated = true;
            }

            return Node.Status.Running;
        }

        public void Reset()
        {
            currentIndex = 0;
        }
    }
}