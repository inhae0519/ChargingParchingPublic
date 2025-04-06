using UnityEngine;
[CreateAssetMenu(fileName = "EnemyRunState", menuName = "SO/EnemyState/EnemyIdleState")]
public class EnemyIdleStateSO : EnemyStateSO
{
    private EnemyAnimator _anim;
    public override void OnEnter(Entity entity)
    {
        _enemy = entity as Enemy;

        if(AnimState !=null)
        {
            _anim = _enemy.GetEntityCompo<EnemyAnimator>();
            _anim.SetBool(AnimState.HashValue, true);
        }

    }

    public override void OnExit()
    {
        if (AnimState != null)
        {
            _anim.SetBool(AnimState.HashValue, false);
        }
    }

    public override void Update()
    {
        
    }

}
