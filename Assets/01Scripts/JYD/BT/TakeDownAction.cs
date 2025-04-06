using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TakeDown", story: "TakeDown", category: "Action", id: "72cca1d57c1af06320e8c885db341dee")]
public partial class TakeDownAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> DownSpeed;
    [SerializeReference] public BlackboardVariable<float> DelayTime;
    [SerializeReference] public BlackboardVariable<GolemBoss> Golem;
    
    private bool isFinished = false;

    protected override Status OnStart()
    {
        isFinished = false;
        Golem.Value.StartCoroutine(TakeDownRoutine());
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return isFinished ? Status.Success : Status.Running;
    }

    private IEnumerator TakeDownRoutine()
    {
        Golem.Value.HandPattern.TakeDownHand(true, Speed.Value, DownSpeed.Value);
        yield return new WaitForSeconds(DelayTime.Value); 
        Golem.Value.HandPattern.TakeDownHand(false, Speed.Value, DownSpeed.Value);
        yield return new WaitForSeconds(DelayTime.Value); 
        
        isFinished = true; 
    }
}