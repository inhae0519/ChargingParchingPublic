using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private PlayerMuzzleEffect _playerMuzzleEffect;
    [SerializeField] private ParticleEmisiionHandler _emisiion;
    [SerializeField] protected GameEventChannelSO _spawnChannel;
    [SerializeField] protected PlayerBullet _bullet;

    [SerializeField] private float _particleMaxSize;
    [SerializeField] private float _recoilTime;
    [SerializeField] private float _recoilXValue;

    [SerializeField] private float _minPower;

    private Vector3 _defaultPos;

    protected Transform _visual;
    protected Transform _fireTrm;
    protected Player _player;

    private ParticleEmisiionHandler _currentEmisiion;
    
    private void Awake()
    {
        _visual = transform.Find("Visual");
        _fireTrm = transform.Find("FirePos");
    }

    public void SetOwner(Player player)
    {
        _player = player;
        
        _player.GetPlayerCompo<PlayerWeaponController>().resetEvent.AddListener(HandleEmmisionReset);
    }

    private void OnDestroy()
    {
        _player.GetPlayerCompo<PlayerWeaponController>().resetEvent.RemoveListener(HandleEmmisionReset);
    }
    
    public virtual void Charging(float chargingTime ,float chargingValue)
    {
        if (_currentEmisiion == null)
        {
            _currentEmisiion = Instantiate(_emisiion, _fireTrm.position, Quaternion.identity);
            _currentEmisiion.transform.SetParent(transform);
        }

        float inverseLerp = Mathf.InverseLerp(0, 7f, chargingTime);
        float size = Mathf.Lerp(0.1f, _particleMaxSize, inverseLerp);
        _currentEmisiion.transform.localScale = new Vector3(size, size, size);
        _currentEmisiion.ChangeEmission(size);
    }

    public virtual void Fire(float power)
    {
        HandleEmmisionReset();
        InstantiateMuzzle();
        Recoil(power);
    }

    private void InstantiateMuzzle()
    {
        PlayerMuzzleEffect effect = Instantiate(_playerMuzzleEffect);
        effect.transform.position = _fireTrm.position;
        float z = Mathf.Atan2(_player.LooDir.y, _player.LooDir.x) * Mathf.Rad2Deg;
        effect.SetRotate(z);
    }

    private void Recoil(float power)
    {
        if(power <= _minPower)
            return;
        
        _defaultPos = _visual.localPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(_visual.DOLocalMoveX(_recoilXValue, _recoilTime));
        seq.Append(_visual.DOLocalMoveX(_defaultPos.x, _recoilTime))
            .OnComplete(() => _visual.localPosition = _defaultPos);
    }
    
    private void HandleEmmisionReset()
    {
        if(_currentEmisiion == null)
            return;

        _currentEmisiion.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => Destroy(_currentEmisiion.gameObject));
    }
}
