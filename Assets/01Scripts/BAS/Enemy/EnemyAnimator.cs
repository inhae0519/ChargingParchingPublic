using UnityEngine;

public class EnemyAnimator : MonoBehaviour,IEntityComponent
{
    private Animator _animator;
    private Enemy _enemy;
    public void Initialize(Entity entity)
    {
        _animator = entity.GetComponent<Animator>();
        _enemy = entity as Enemy;
    }

    public void SetBool(int id, bool value)
    {
        _animator.SetBool(id, value);
    }
}
