using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponController : MonoBehaviour, IPlayerCompo
{
    [Header("Sound")]
    [SerializeField] private GameEventChannelSO _soundChannelSo;
    [SerializeField] private SoundSO _shotSound;
    [SerializeField] private SoundSO _chargingSound;
    
    public UnityEvent<float, float> chargingEvent;
    public UnityEvent<float> fireEvent;
    public UnityEvent resetEvent;

    public List<float> levelSeconds;
    public List<float> levelAdditiveValue;
    
    [SerializeField] private float _minChargingValue;
    [SerializeField] private float _chargingDelayTime;
    
    public Weapon currentWeapon;
    
    private Player _player;
    private PlayerInputSO _playerInput;
    private PlayerRender _playerRender;
    
    private bool _isChargingStart;
    public bool IsChargingStart => _isChargingStart;

    private float _currentCharging;
    private float _currentChargingTime;
    private float _currentChargingDelayTime;
    public int CurrentLevelIndex { get; private set; }
    
    private Dictionary<Type, Weapon> _weapons;
    private bool _onceChargingSound;
    
    public void Initialize(Player player)
    {
        _player = player;
        _playerInput = player.PlayerInput;
        _playerRender = player.GetPlayerCompo<PlayerRender>();

        _weapons = new Dictionary<Type, Weapon>();
        GetComponentsInChildren<Weapon>(true).ToList().ForEach(x => _weapons.Add(x.GetType(), x));
        WeaponChange<DefaultWeapon>();
        
        _playerInput.AttackEvent += HandleChargingEvent;
        _playerInput.SlotChangeEvent += HandleSlotChange;
    }

    private void OnDestroy()
    {
        _playerInput.AttackEvent -= HandleChargingEvent;
        _playerInput.SlotChangeEvent -= HandleSlotChange;
    }

    private void HandleSlotChange(int index)
    {
        if(_isChargingStart)
            return;
        
        if(index==0)
            WeaponChange<DefaultWeapon>();
        else
            WeaponChange<ShotGunWeapon>();
    }

    private void HandleChargingEvent(bool isCharging)
    {
        _isChargingStart = isCharging;
        if (_onceChargingSound == false)
        {
            var evt = SoundEvents.PlayLoopSFXEvent;
            evt.clipData = _chargingSound;
            _soundChannelSo.RaiseEvent(evt);
            _onceChargingSound = true;
        }
        
        if (!isCharging)
        {
            _onceChargingSound = false;
            var evt2 = SoundEvents.StopLoopSFXEvent;
            _soundChannelSo.RaiseEvent(evt2);
            
            if (_currentCharging <= _minChargingValue)
                resetEvent?.Invoke();
            else
            {
                var evt = SoundEvents.PlaySfxEvent;
                evt.clipData = _shotSound;
                
                _soundChannelSo.RaiseEvent(evt);
                fireEvent?.Invoke(_currentCharging);
            }
            
            _currentChargingTime = 0f;
            _currentCharging = 0f;
            _currentChargingDelayTime = 0f;
        }
    }

    private void Update()
    {
        if (_isChargingStart)
        {
            ChargingLogic();
            chargingEvent?.Invoke(_currentChargingTime, _currentCharging);
        }
        GunRotate();
    }

    private void ChargingLogic()
    {
        _currentChargingTime += Time.deltaTime;
        
        float maxValue = levelSeconds.Where(x => x <= _currentChargingTime).Max();
        CurrentLevelIndex = levelSeconds.FindLastIndex(x => x <= maxValue);
        
        _currentChargingDelayTime -= Time.deltaTime;
        if (_currentChargingDelayTime <= 0)
        {
            float curIncValue = levelAdditiveValue[CurrentLevelIndex];

            curIncValue = _playerInput.InputDirection == Vector2.zero ? curIncValue : 1;
            
            _currentCharging += curIncValue;
            _currentChargingDelayTime = _chargingDelayTime;
        }
    }
    
    private void GunRotate()
    {
        float z = Mathf.Atan2(_player.LooDir.y, _player.LooDir.x) * Mathf.Rad2Deg;
        if (_playerRender.FacingDirection <= 0f)
            z = -z + 180f;
        
        currentWeapon.transform.localEulerAngles = new Vector3(0,0, z);
    }

    private void WeaponChange<T>()
    {
        if (currentWeapon != null)
        {
            fireEvent.RemoveListener(currentWeapon.Fire);
            chargingEvent.RemoveListener(currentWeapon.Charging);
            currentWeapon.gameObject.SetActive(false);
        }
        
        currentWeapon = _weapons[typeof(T)];
        currentWeapon.gameObject.SetActive(true);
        
        currentWeapon.SetOwner(_player);
        fireEvent.AddListener(currentWeapon.Fire);
        chargingEvent.AddListener(currentWeapon.Charging);
    }

    public void SetActiveWeapon(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void InvokeResetEvent()
    {
        resetEvent?.Invoke();
    }
}
