using System;
using UnityEngine;

public class SmokeParticle : MonoBehaviour,IPoolable
{
    public PoolType Type => _poolType;
    public GameObject GameObject => gameObject;

    [SerializeField] private PoolType _poolType;
    private Pool _myPool;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float lifeTime;
    private float timer;


    public void PlayParticle()
    {
        _particleSystem.Simulate(0);
        _particleSystem.Play();
    }

    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            _myPool.Push(this);
        }
    }

    public void ResetItem()
    {
        timer = 0;
    }
}
