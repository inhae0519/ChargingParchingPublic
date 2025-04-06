using System;
using Unity.AppUI.UI;
using Unity.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanAttack", story: "Can Attack [CoolDown]", category: "Conditions", id: "2e1523c99d4b96e907b0f7f3ba7bcef6")]
public partial class CanAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CoolDown;
    private float lastAttackTime;
        
    public override bool IsTrue()
    {
        int rand = Random.Range(0,10);
        
        if (lastAttackTime + CoolDown.Value < Time.time || rand < 2)
        {
            lastAttackTime = Time.time;
            return true;
        }
        
        return false;
    }

   

}
