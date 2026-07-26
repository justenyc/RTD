using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Not Null", story: "[TransformVar] is not null", category: "Conditions", id: "d7dca700f82813a104c639d947325c4b")]
public partial class NotNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> TransformVar;

    public override bool IsTrue()
    {
        return TransformVar.Value != null;
    }
}