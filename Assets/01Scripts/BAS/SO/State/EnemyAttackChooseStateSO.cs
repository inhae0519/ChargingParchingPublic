using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyRunState", menuName = "SO/EnemyState/EnemyChooseState")]
public class EnemyAttackChooseStateSO : EnemyStateSO
{
    public List<EnemyStateSO> ChooseList = new List<EnemyStateSO>();

    
    public override void OnEnter(Entity entity)
    {
        _enemy = entity as Enemy;

        NextState = ChooseList[Random.Range(0,ChooseList.Count)];

        DoExit();
    }

    public override void OnExit()
    {
       
    }

    public override void Update()
    {
        
    }
}
