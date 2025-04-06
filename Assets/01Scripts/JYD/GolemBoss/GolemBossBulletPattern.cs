using System;
using System.Collections;
using UnityEngine;

public class GolemBossBulletPattern : MonoBehaviour
{
    private GolemBoss golemBoss;
    
    [SerializeField] private GameEventChannelSO SpawnChanel;
    
    [Space]
    [SerializeField] private Transform chest;
    [SerializeField] private Transform leftSolder;
    [SerializeField] private Transform rightSolder;

    [Header("Laser")]
    [SerializeField] private Laser leftLaser;
    [SerializeField] private Laser rightLaser;

    private void Awake()
    {
        golemBoss = GetComponent<GolemBoss>();
    }

    public void ApplyCircleShot(Transform _firePos, int bulletCount, float _angle = 360)
    {
        float angleStep = _angle / bulletCount;
        float angle = _firePos.rotation.eulerAngles.z; 

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            var evt = SpawnEvents.BulletCreate;
            evt.position = _firePos.position;
            evt.dir = bulletMoveDirection;
            evt._bulletType = PoolType.EnemyBullet;
            evt.damage = 5;
            evt.speed = 0.5f;
            
            SpawnChanel.RaiseEvent(evt);
            
            angle += angleStep; 
        }
    }
    
    public void SpinShot(int bulletCount,bool isLeft)
    {
        StartCoroutine(ApplySpinShot(isLeft ? leftSolder : rightSolder, bulletCount,bulletCount));
    }
    private IEnumerator ApplySpinShot(Transform _firePos,int bulletCount,int _bulletAmount)
    {
        float angleStep = 20;
        float angle = 270f;
        
        for (int i = 0; i < _bulletAmount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            var evt = SpawnEvents.BulletCreate;
            evt.position = _firePos.position;
            evt.dir = bulletMoveDirection;
            evt._bulletType = PoolType.EnemyBullet;
            evt.damage = 5;
            evt.speed = 0.5f;
            
            SpawnChanel.RaiseEvent(evt);
            
            angle += angleStep;
            yield return new WaitForSeconds(0.01f);
        }        
    }
    public void SectorFormShot(float _angle , int _bulletCount,float _time) 
    {
        if(chest != null)
            StartCoroutine(ApplySectorFormShot(chest , _angle ,_bulletCount ,_time));
    } 
    private IEnumerator ApplySectorFormShot(Transform _firePos, float _angle, int _bulletCount, float _shotTime)
    {
        float elapsedTime = 0f;
        float lastShotTime = 0f;

        float downAngle = 270f;
        float halfAngle = _angle / 2f;
        
        float startAngle = downAngle + halfAngle;
        float endAngle = downAngle - halfAngle;
        
        _firePos.rotation = Quaternion.Euler(0, 0, startAngle);
        
        while (elapsedTime <= _shotTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / _shotTime;
            float rotationAngle = Mathf.Lerp(startAngle , endAngle, progress);

            _firePos.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        
            if (elapsedTime - lastShotTime >= 0.2f)
            {
                ApplyCircleShot(_firePos, _bulletCount, halfAngle);
                lastShotTime = elapsedTime;
            }
            
            yield return null;
        }
    }
    public void ActiveLaser(bool isActive)
    {
        leftLaser.gameObject.SetActive(isActive);
        rightLaser.gameObject.SetActive(isActive);
    }
        
}