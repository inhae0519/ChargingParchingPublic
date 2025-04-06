using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
[CreateAssetMenu(fileName = "EnemyRunState", menuName = "SO/EnemyState/EnemyAttackState")]
public class EnemyMeleeAttackStateSO : EnemyStateSO
{
    [SerializeField]
    private AnimStateSO _attackParm;
    public override void OnEnter(Entity entity)
    {
        _enemy = entity as Enemy;
        if (_statModifier != null)
            entity.GetEntityCompo<EntityStat>().AddModifier(_statModifier.TargetStat, _statModifier, _statModifier.Value);
        entity.GetComponentInChildren<Animator>().SetBool(_attackParm.HashValue, true);
    }

    public override void OnExit()
    {
        if (_statModifier != null)
            _enemy.GetEntityCompo<EntityStat>().RemoveModifier(_statModifier.TargetStat, _statModifier);
        _enemy.GetComponentInChildren<Animator>().SetBool(_attackParm.HashValue, false);
    }

    public override void Update()
    {
        
    }


}
