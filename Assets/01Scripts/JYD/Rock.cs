using DG.Tweening;
using UnityEngine;

public class Rock : MonoBehaviour,IPoolable
{
    public PoolType Type => _poolType;
    public GameObject GameObject => gameObject;

    private Pool _myPool;
    
    [SerializeField] private PoolType _poolType;
    [SerializeField] private GameEventChannelSO ChannelSo;
    [SerializeField] private LayerMask whatIsTarget;
    
    private bool isDie;
    
    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void ResetItem()
    {
        isDie = false;
    }
    
    public void SetDirection(Vector2 direction,float _fallTime)   
    {
        transform.DOMove(direction,_fallTime).SetEase(Ease.InSine);
    }
        
    public void StartExplosion()
    {
        if(isDie)return;

        isDie = true;
        
        var evt = SpawnEvents.ExplosionCreate;
        evt.position = transform.position;
        evt.poolType = PoolType.ExplosionParticle;
        
        ChannelSo.RaiseEvent(evt);
        
        float explosionRadius = 5f; 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsTarget);
    
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<IDamageable>(out var damageable) && hitCollider.GetComponent<Player>())
            {
                damageable.ApplyDamage(5f); 
            }
        }
        
        _myPool.Push(this);
    }
}
