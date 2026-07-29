using Unity.Behavior;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(SphereCollider))]
public class BHV_EnemyVision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BehaviorGraphAgent bGraph;
    [SerializeField] string bGraphVarName = "Stalk Target";
    [SerializeField] SphereCollider visionRadius;

    [Header("Settings")]
    [SerializeField] float searchInterval = 1f;
    [SerializeField] LayerMask visionMask;

    Color debugColor = Color.white;
    [Header("Debug")]
    [SerializeField] HashSet<Collider> collidersInRadius = new();
    [SerializeField] float searchTick;
    [SerializeField] Transform stalkTarget;

    private void OnValidate()
    {
        bGraph = bGraph ?? GetComponent<BehaviorGraphAgent>();
        visionRadius = visionRadius ?? GetComponent<SphereCollider>();
        visionRadius.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        collidersInRadius.Add(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (searchTick > 0)
        {
            searchTick -= Time.deltaTime;
            return;
        }

        foreach (Collider c in collidersInRadius)
        {
            if (stalkTarget != null)
            {
                return;
            }

            CheckVision(c);
        }
        searchTick = searchInterval;
    }

    void CheckVision(Collider check)
    {
        RaycastHit hit;
        Vector3 dir = check.transform.position - transform.position;
        if (Physics.Raycast(transform.position + dir.normalized, dir, out hit, dir.magnitude, visionMask))
        {
            Logger.LogMessage($"{hit.collider} : {check}");
            if (hit.collider == check)
            {
                Logger.DrawRay(transform.position, dir, Color.green);
                stalkTarget = hit.collider.transform;
                bGraph.BlackboardReference.SetVariableValue(bGraphVarName, stalkTarget);
                return;
            }

            Logger.DrawRay(transform.position, hit.point - transform.position, Color.red);
            stalkTarget = null;
            bGraph.BlackboardReference.SetVariableValue<Transform>(bGraphVarName, null);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidersInRadius.Remove(other);
    }
}