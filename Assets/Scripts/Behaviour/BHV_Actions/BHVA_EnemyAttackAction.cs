using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BHVA_EnemyAttack", story: "[Self] spawns [Hitbox]", category: "Action", id: "76234bbb6eb99852c97deb4881c05b0b")]
public partial class BHVA_EnemyAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Hitbox;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<float> ActiveTime = new(1f);

    float timeTilEnd;

    protected override Status OnStart()
    {
        Animator.Value.SetTrigger("Attack");
        //Hitbox.Value.SetActive(true);
        timeTilEnd = ActiveTime;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (timeTilEnd > 0)
        {
            timeTilEnd -= Time.deltaTime;
            return Status.Running;
        }

        return Status.Success;
    }

    protected override void OnEnd()
    {
        Animator.Value.SetTrigger("TEMP_Return");
        //Hitbox.Value.SetActive(false);
    }
}

