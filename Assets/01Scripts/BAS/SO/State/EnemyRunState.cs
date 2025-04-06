using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRunState", menuName = "SO/EnemyState/EnemyRunState")]
public class EnemyRunState : EnemyStateSO //도망가기 상태
{

    [SerializeField]
    private float _distance = 5f;
    [SerializeField]
    private bool _isPierceObstacle = false;
    [SerializeField]
    private LayerMask _whatisObstacle;
    private BashAstar _astar;
    private EnemyAnimator _anim;
    public override void OnEnter(Entity entity)
    {
        _enemy = entity as Enemy;

        if( _statModifier != null )
        entity.GetEntityCompo<EntityStat>().AddModifier(_statModifier.TargetStat, _statModifier, _statModifier.Value);

        _astar = _enemy.GetEntityCompo<BashAstar>();

        _anim = _enemy.GetEntityCompo<EnemyAnimator>();

        _anim.SetBool(AnimState.HashValue, true);
    }

    public override void OnExit()
    {
        if (_statModifier != null)
            _enemy.GetEntityCompo<EntityStat>().RemoveModifier(_statModifier.TargetStat, _statModifier);
        _anim.SetBool(AnimState.HashValue, false);
    }

    public override void Update()
    {
        Vector2 targetpos = _playerSO.PlayerTrm.position + (_enemy.transform.position-_playerSO.PlayerTrm.position) * _distance; ;
        if(_isPierceObstacle )
           {
            RaycastHit2D hit = Physics2D.Raycast(_playerSO.PlayerTrm.position, (_playerSO.PlayerTrm.position - _enemy.transform.position), _distance,_whatisObstacle);
            if(hit)
                targetpos = hit.point;
            } 
        _astar.Target = targetpos;
        Vector2 dir = _astar.PathDir;
        _enemy.GetEntityCompo<EnemyMovement>().Move(dir);
    }
}
