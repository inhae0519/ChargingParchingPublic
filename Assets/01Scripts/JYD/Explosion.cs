using UnityEngine;

public class Explosion : MonoBehaviour,IPoolable
{
    public PoolType Type => _poolType;
    public GameObject GameObject => gameObject;

    public float lifeTime;
    private float lifeTimer;
    private Pool _myPool;

    [SerializeField] private PoolType _poolType;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private SoundSO explosionSO;
    [SerializeField] private GameEventChannelSO soundChannel;
    
    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            _myPool.Push(this);
        }
    }

    public void PlayParticle()
    {
        _particleSystem.Simulate(0);
        _particleSystem.Play();

        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = explosionSO;
        
        soundChannel.RaiseEvent(evt);
    }
    
    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void ResetItem()
    {
        lifeTimer = 0;
    }
}