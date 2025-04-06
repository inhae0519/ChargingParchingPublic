using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LaserShotOff", story: "LaserShotOff", category: "Action", id: "3a0af3302150bbfff835aef7c2f9674d")]
public partial class LaserShotOffAction : Action
{
    
    [SerializeReference] public BlackboardVariable<GolemBoss> GolemBoss;
    protected override Status OnStart()
    {
        GolemBoss.Value.BulletPattern.ActiveLaser(false);   
        
        return Status.Success;
    }

    
}

