using UnityEngine;

public enum PoolType
{
    PlayerBullet,
    EnemyBullet,
    SoundPlayer,
    ImpactParticle,
    ExplosionParticle,
    Rock,
    HitImpact,
    EnemyHomingBullet,
    EnemyFastBullet,
    PopUpText,
}

public interface IPoolable
{
    public PoolType Type { get; }
    public GameObject GameObject { get; }
    public void SetUpPool(Pool pool);
    public void ResetItem();
}
