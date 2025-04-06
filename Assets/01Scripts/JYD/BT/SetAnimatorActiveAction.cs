using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Assertions.Must;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Animator Active", story: "Set Animator [Active]", category: "Action", id: "84cb63aaaeba68316e664216c099ba63")]
public partial class SetAnimatorActiveAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<bool> Active;

    protected override Status OnStart()
    {
        Animator.Value.enabled = Active.Value;
        
        return Status.Success;
    }

  
}

