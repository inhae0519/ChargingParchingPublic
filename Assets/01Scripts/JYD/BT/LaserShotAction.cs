using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LaserShot", story: "LaserShot", category: "Action", id: "6992d157df8d30d01a9b531584dfc6c9")]
public partial class LaserShotAction : Action
{
    [SerializeReference] public BlackboardVariable<GolemBoss> Golem;
    
    protected override Status OnStart()
    {
        Golem.Value.BulletPattern.ActiveLaser(true);
        
        return Status.Success;
    }


}

