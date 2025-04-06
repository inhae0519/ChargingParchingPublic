using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CrossHand", story: "CrossHand", category: "Action", id: "68b82e40dbd6276f8f68c44b8af6edeb")]
public partial class CrossHandAction : Action
{

    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> CrossSpeed;
    
    [SerializeReference] public BlackboardVariable<GolemBoss> Golem;
    
    protected override Status OnStart()
    {
        Golem.Value.HandPattern.CrossHands(Speed.Value , CrossSpeed.Value);
        return Status.Success;
    }

    
}

