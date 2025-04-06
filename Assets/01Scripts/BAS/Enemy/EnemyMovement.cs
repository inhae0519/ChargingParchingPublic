using UnityEngine;

public class EnemyMovement : MonoBehaviour,IEntityComponent
{
    private Entity _entity;
    private Rigidbody2D _rbCompo;
    private EntityStat _entityStat;
    [SerializeField]
    private StatSO _speedStat;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        _rbCompo = _entity.GetComponentInChildren<Rigidbody2D>();
        _entityStat = _entity.GetEntityCompo<EntityStat>();
    }

    public void Move(Vector2 dir)
    {
        float maxSpeed =25;
        if(_entityStat.TryGetStat(_speedStat,out StatSO stat))
        {
            maxSpeed = stat.Value;
        }
        _rbCompo.AddForce(BashUtils.LimitedSpeed(_rbCompo.linearVelocity,dir*maxSpeed/3f,maxSpeed),ForceMode2D.Impulse);
    }
    public void Knockback(Vector2 dir)
    {
        _rbCompo.AddForce(dir,ForceMode2D.Impulse);
    }
    public void SetVelocity(Vector2 dir)
    {
        _rbCompo.linearVelocity = dir;
    }
}
