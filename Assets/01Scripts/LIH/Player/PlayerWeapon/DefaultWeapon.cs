using UnityEngine;

public class DefaultWeapon : Weapon
{
    [SerializeField] private float _maxSize = 3f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _damage = 10f;

    public override void Charging(float chargingTime, float chargingSpeed)
    {
        base.Charging(chargingTime,chargingSpeed);
    }

    public override void Fire(float power)
    {
        var evt = SpawnEvents.BulletCreate;
        evt._bulletType = PoolType.PlayerBullet;
        evt.damage = _damage + power/1.5f;
        evt.dir = _player.LooDir * Mathf.Min(1+power/4 , 12);
        evt.position = _fireTrm.position;
        
        float sizet = Mathf.InverseLerp(0, 100f, power);
        evt.size = Mathf.Lerp(1f, _maxSize, sizet);
        
        float speed = Mathf.Lerp(0, 1000f, 0.05f+power/100);
        evt.speed = Mathf.Lerp(1f, _maxSpeed, speed);
        
        _spawnChannel.RaiseEvent(evt);
        base.Fire(power);
    }
}
