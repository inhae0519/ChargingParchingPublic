using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField] private float _damage;
    public Transform attackCheckerTrm;
    public float attackCheckerRadius;

    private int _maxHitCount = 1; //최대로 때릴 수 있는 적의 수
    public LayerMask whatIsEnemy;
    private Collider2D[] _hitResult;

    private Entity _owner;

    private void Awake()
    {
        _hitResult = new Collider2D[_maxHitCount];
    }

    public void SetOwner(Entity owner)
    {
        _owner = owner;
    }
    
    public void CastDamage()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(whatIsEnemy);

        int cnt = Physics2D.OverlapCircle(
            attackCheckerTrm.position, 
            attackCheckerRadius, 
            filter, 
            _hitResult);

        if (cnt > 0)
        {
            if (_hitResult[0].TryGetComponent(out IDamageable target))
            {
                target.ApplyDamage(_damage);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackCheckerTrm != null)
        {
            Gizmos.DrawWireSphere(attackCheckerTrm.position, attackCheckerRadius);
        }
    }
#endif
}
