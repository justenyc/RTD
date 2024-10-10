using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;

public interface IStrategy
{
    Node.Status Process();
    void OnTransition() { }
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

    public class DelayUntil : IStrategy
    {
        readonly Func<bool> predicate;

        public DelayUntil(Func<bool> _predicate)
        {
            predicate = _predicate;
        }

        public Node.Status Process()
        {
            if(!predicate())
            {
                return Node.Status.Running;
            }
            return Node.Status.Success;
        }
    }

    public class DelayForSeconds : IStrategy
    {
        float duration;
        float time = 0;

        public DelayForSeconds(float _duration)
        {
            duration = _duration;
        }

        public Node.Status Process()
        {
            if(time < duration)
            {
                time += Time.deltaTime;
                return Node.Status.Running;
            }
            Reset();
            return Node.Status.Success;
        }

        public void Reset()
        {
            time = 0;
        }
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
        //float patrolSpeed;

        int currentIndex;
        bool isPathCalculated;

        public MoveBetweenPoints(NavMeshAgent _agent, List<Vector3> _patrolPoints, float _patrolSpeed = 2f)
        {
            agent = _agent;
            patrolPoints = _patrolPoints;
            //patrolSpeed = _patrolSpeed;
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
        Blackboard blackboard;
        float range;

        public Stalk(NavMeshAgent _agent, Blackboard _blackBoard, float _range) 
        {
            agent= _agent;
            blackboard = _blackBoard;
            range = _range;
        }

        public Node.Status Process()
        {
            blackboard.SetValue(Shambler.TRACK_ROTATION, true);
            var target = blackboard.GetEntryByKey<Vector3>(Shambler.DETECTED_POSITION) as BlackboardEntry<Vector3>;

            //Debug.Log($"{range} : {Vector3.Distance(agent.transform.position, target.value)}");
            if (Vector3.Distance(agent.transform.position, target.value) > range)
            {
                agent.SetDestination(target.value);
                return Node.Status.Running;
            }
            return Node.Status.Success;
        }
    }

    public class TimedLoop : IStrategy
    {
        float timePerTick;
        float currentTime = 0;
        Action action;

        public TimedLoop(float _timePerTick, Action _action)
        {
            timePerTick = _timePerTick;
            action = _action;
        }

        public Node.Status Process()
        {
            if(currentTime > timePerTick)
            {
                currentTime = 0;
                action();
            }
            currentTime += Time.deltaTime;
            return Node.Status.Running;
        }
    }
}