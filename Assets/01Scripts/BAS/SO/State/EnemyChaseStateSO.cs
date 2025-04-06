using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
[CreateAssetMenu(fileName = "EnemyRunState", menuName = "SO/EnemyState/EnemyChaseState")]
public class EnemyChaseStateSO : EnemyStateSO // 쫒아가기 상태
{

    private BashAstar _astar;
    [SerializeField]
    private float _mindistance = 2;
    private EnemyAnimator _anim;
    public override void OnEnter(Entity entity)
    {
        _enemy = entity as Enemy;
        
        entity.GetEntityCompo<EntityStat>().AddModifier(_statModifier.TargetStat,_statModifier,_statModifier.Value);
        _astar = _enemy.GetEntityCompo<BashAstar>();
    }

    public override void OnExit()
    {
        _enemy.GetEntityCompo<EntityStat>().RemoveModifier(_statModifier.TargetStat, _statModifier);
    }

    public override void Update()
    {
        _astar.Target = _playerSO.PlayerTrm.position;
        Vector2 dir = _astar.PathDir;
        _enemy.GetEntityCompo<EnemyMovement>().Move(dir);
    }
}
