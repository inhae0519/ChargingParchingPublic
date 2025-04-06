using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour, IPlayerCompo
{
    public UnityEvent<float, float> _dashCoolEvent;
    
    [Header("Sound")]
    [SerializeField] private GameEventChannelSO _soundChannelSo;
    [SerializeField] private SoundSO _dashSound;
    
    [Header("Stat")]
    [SerializeField] private StatSO _moveSpeedStat;
    [SerializeField] private StatSO _dashSpeedStat;
    [SerializeField] private StatSO _dashCoolStat;

    [Header("Dash Setting")]
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashInvincibilityTime;
    
    [Header("knockBack Setting")]
    [SerializeField] private float _knockBackTime;
    [SerializeField] private float _applyKnockBackMaxPower;
    [SerializeField] private float _maxKnockBackChargingValue;
    [SerializeField] private float _knockBackMinPower;
    
    [Header("charging speed Setting")]
    [SerializeField] private float _zeroSpeedChargingValue;
    
    private Player _player;
    private PlayerWeaponController _playerWeaponController;
    private PlayerRender _playerRender;
    private Health _health;
    
    private Rigidbody2D _rigidbody2D;
    private EntityStat _statCompo;

    [field: SerializeField] public bool CanMove { get; set; } = true;

    private Vector2 _moveDir;

    private float _moveSpeed;
    private float _dashSpeed;
    private float _currentDashCool;
    private float _dashCoolTime;
    
    private float _chargingMoveMultiplier = 1f;

    public void Initialize(Player player)
    {
        _player = player;
        _playerWeaponController = player.GetPlayerCompo<PlayerWeaponController>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        _statCompo = _player.GetEntityCompo<EntityStat>();
        _playerRender = player.GetPlayerCompo<PlayerRender>();
        _health = player.GetEntityCompo<Health>();
        
        _moveSpeed = _statCompo.GetStat(_moveSpeedStat).Value;
        _dashSpeed = _statCompo.GetStat(_dashSpeedStat).Value;
        _dashCoolTime = _statCompo.GetStat(_dashCoolStat).Value;

        _player.PlayerInput.DashEvent += HandleDashEvent;
        _playerWeaponController.chargingEvent.AddListener(HandleSetChargingSpeed);
        _playerWeaponController.fireEvent.AddListener(HandleResetChargingSpeed);
        _playerWeaponController.resetEvent.AddListener(ResetCharging);
    }

    private void OnDestroy()
    {
        _player.PlayerInput.DashEvent -= HandleDashEvent;
        _playerWeaponController.chargingEvent.RemoveListener(HandleSetChargingSpeed);
        _playerWeaponController.fireEvent.RemoveListener(HandleResetChargingSpeed);
        _playerWeaponController.resetEvent.RemoveListener(ResetCharging);
    }

    private void Update()
    {
        if (_currentDashCool >= 0)
        {
            _currentDashCool -= Time.deltaTime;
            _dashCoolEvent?.Invoke(_currentDashCool, _dashCoolTime);
        }
    }

    private void HandleDashEvent()
    {
        if(_currentDashCool >= 0)
            return;

        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _dashSound;
        _soundChannelSo.RaiseEvent(evt);
        
        _health.SetInvincibility(true);
        _playerRender.StartSande(_dashTime);
        CanMove = false;
        _currentDashCool = _dashCoolTime;
        StopImmediately(true);

        Vector2 dir = SetDashDir();
        _rigidbody2D.linearVelocity = dir * _dashSpeed;
    
        DOVirtual.DelayedCall(_dashInvincibilityTime, () => _health.SetInvincibility(false));
        DOVirtual.DelayedCall(_dashTime, () => CanMove = true);
    }

    private Vector2 SetDashDir()
    {
        Vector2 dir =  Vector2.zero;
        switch (_player.dashType)
        {
            case PlayerDashType.MouseDir:
                dir = _player.LooDir;
                break;
            case PlayerDashType.InputDir:
                dir = _player.PlayerInput.InputDirection.normalized;
                break;
        }

        return dir;
    }

    public void KnockBackStart(float power)
    {
        StartCoroutine(KnockBack(power));
    }
    
    private IEnumerator KnockBack(float power)
    {
        if(power <= _knockBackMinPower)
            yield break;
        
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        
        CanMove = false;
        StopImmediately(true);
        
        float inverseLerp = Mathf.InverseLerp(0, _maxKnockBackChargingValue, power);
        float knockBackPower = Mathf.Lerp(0, _applyKnockBackMaxPower, inverseLerp);
        
        _rigidbody2D.AddForce(-_player.LooDir * knockBackPower, ForceMode2D.Impulse);
        DOVirtual.DelayedCall(_knockBackTime, () => CanMove = true);
    }

    private void FixedUpdate()
    {
        if(!CanMove)
            return;
        
        _moveDir = _player.PlayerInput.InputDirection;
        _rigidbody2D.linearVelocity = _moveDir * (_moveSpeed * _chargingMoveMultiplier);
    }
    
    private void StopImmediately(bool isYAxisToo = false)
    {
        if(isYAxisToo)
            _rigidbody2D.linearVelocity = Vector2.zero;
        else
            _rigidbody2D.linearVelocityX = 0;

        _moveDir = Vector2.zero;
    }

    public void SetActiveCanMove(bool _isActive)
    {
        CanMove = _isActive;
        StopImmediately();
    }

    private void HandleResetChargingSpeed(float power)
    {
        ResetCharging();
    }

    private void ResetCharging()
    {
        CanMove = true;
        _chargingMoveMultiplier = 1f;
    }

    private void HandleSetChargingSpeed(float chargingTime, float chargingValue)
    {
        if (CanMove == false)
            return;

        chargingValue = Mathf.Min(chargingValue, 100);
        
        _chargingMoveMultiplier = Mathf.Lerp(1.0f, 0.3f, chargingValue / 100f);
    }

}
