using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpinShot", story: "SpinShot", category: "Action", id: "72a3d56f9f1d265dae5221c2cfc7473d")]
public partial class SpinShotAction : Action
{
    [SerializeReference] public BlackboardVariable<GolemBoss> Golem;
    
    [SerializeReference] public BlackboardVariable<int> BulletCount;
    [SerializeReference] public BlackboardVariable<bool> IsLeft;
    protected override Status OnStart()
    {
        Golem.Value.BulletPattern.SpinShot(BulletCount.Value,IsLeft.Value);
        
        return Status.Success;
    }

   
}

