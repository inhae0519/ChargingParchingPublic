using UnityEngine;

public class ShotGunWeapon : Weapon
{
    [SerializeField] private float _oneBulletPower;
    [SerializeField] private float _spreadValue;
    
    [SerializeField] private float _maxSize = 1.25f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _damage = 2f;

    public override void Charging(float chargingTime, float chargingSpeed)
    {
        base.Charging(chargingTime,chargingSpeed);
    }

    public override void Fire(float power)
    {
        float bulletCount = power / _oneBulletPower/2+5;

        for (int i = 0; i < bulletCount; i++)
        {
            var evt = SpawnEvents.BulletCreate;
            evt._bulletType = PoolType.PlayerBullet;
            evt.damage = _damage*(1 + power/10);

            Vector2 dir = _player.LooDir;
            float x = Random.Range(-_spreadValue, _spreadValue);
            float y = Random.Range(-_spreadValue, _spreadValue);
            dir.x += x;
            dir.y += y;
            
            evt.dir = dir.normalized * Mathf.Min(1+power/4 , 12);
                        
            evt.position = _fireTrm.position;
                        
            float sizet = Mathf.InverseLerp(0, 100f, power);
            evt.size = Mathf.Lerp(1f, _maxSize, sizet);
        
            //float speed = Mathf.InverseLerp(0, 100f, power);
            //evt.speed = Mathf.Lerp(1f, _maxSpeed, speed);
            
            _spawnChannel.RaiseEvent(evt);
        }
        base.Fire(power);
    }
}
