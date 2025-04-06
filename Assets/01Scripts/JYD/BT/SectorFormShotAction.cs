using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SectorFormShot", story: "SectorFormShot [BulletCount] In [Angle] for [Time]", category: "Action", id: "09b94ac54a0aca0320660875bbadaad8")]
public partial class SectorFormShotAction : Action
{
    [SerializeReference] public BlackboardVariable<int> BulletCount;
    [SerializeReference] public BlackboardVariable<float> Angle;
    [SerializeReference] public BlackboardVariable<float> Time;
    [SerializeReference] public BlackboardVariable<GolemBoss> Golem;
    
    protected override Status OnStart()
    {
        Golem.Value.BulletPattern.SectorFormShot(Angle.Value ,BulletCount.Value,Time.Value);
        return Status.Success;
    }
    
}

