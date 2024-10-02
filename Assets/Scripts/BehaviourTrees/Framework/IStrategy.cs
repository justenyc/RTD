using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public class MoveBetweenPoints : IStrategy
    {
        NavMeshAgent agent;
        List<Vector3> patrolPoints;
        float patrolSpeed;

        int currentIndex;
        bool isPathCalculated;

        public MoveBetweenPoints(NavMeshAgent _agent, List<Vector3> _patrolPoints, float _patrolSpeed = 2f)
        {
            agent = _agent;
            patrolPoints = _patrolPoints;
            patrolSpeed = _patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count)
            {
                Reset();
                return Node.Status.Success;
            }

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target);

            var distance = Vector3.Distance(agent.transform.position - Vector3.up * agent.baseOffset, agent.destination);
            if(isPathCalculated && distance < 0.1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }

            if (!agent.pathPending)
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

    public class Stalk : IStrategy
    {
        NavMeshAgent agent;
        Vector3 destination;

        public Stalk(NavMeshAgent _agent, Vector3 _destination) 
        {
            agent= _agent;
            destination = _destination;
        }

        public Node.Status Process()
        { 
            throw new NotImplementedException();
        }
    }
}