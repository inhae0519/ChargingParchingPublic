using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CircleShot", story: "CircleShot [bulletCount]", category: "Action", id: "01eee6f622f063ae1daf591d467547bd")]
public partial class CircleShotAction : Action
{
    [SerializeReference] public BlackboardVariable<float> GoSpeed;
    [SerializeReference] public BlackboardVariable<float> DownSpeed;
    
    [SerializeReference] public BlackboardVariable<int> BulletCount;
    [SerializeReference] public BlackboardVariable<GolemBoss> Golem;
    
    [SerializeReference] public BlackboardVariable<Pattern> pattern;
    
    protected override Status OnStart()
    {
        Golem.Value.HandPattern.TakeDownHandAndShoot(true , BulletCount.Value , GoSpeed.Value , DownSpeed.Value , pattern.Value);
        Golem.Value.HandPattern.TakeDownHandAndShoot(false , BulletCount.Value , GoSpeed.Value , DownSpeed.Value , pattern.Value);
        return Status.Success;
    }
}

