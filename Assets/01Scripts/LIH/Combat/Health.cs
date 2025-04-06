using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour, IDamageable, IEntityComponent, IAfterInitable
{
    [SerializeField] private GameEventChannelSO _spawnEventChannel;
    
    [SerializeField] private StatSO _healthStat;
    private float _maxHealth;
    private float _currentHealth;

    public UnityEvent<float> OnHit;
    public UnityEvent OnDead;
    public UnityEvent OnInvincibilityEvent;

    private float _damage;

    private Entity _owner;

    private bool _isInvincibility;
    
    public bool IsDead { get; set; }
    
    private float _lastSpawnTime = -1f; 
    
    private void Update()
    {
        /*if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            ApplyDamage(3f);
            Debug.Log(_currentHealth);
        }*/
    }

    public void Initialize(Entity entity)
    {
        _owner = entity;
    }

    public void SetInvincibility(bool isActive) => _isInvincibility = isActive;
    
    public void AfterInit()
    {
        SetHealth(true);
    }

    private void SetHealth(bool isResetCurrent = false)
    {
        _maxHealth = _owner.GetEntityCompo<EntityStat>().GetStat(_healthStat).Value;
        if (isResetCurrent)
            _currentHealth = _maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        if (IsDead)
            return;

        if (_isInvincibility)
        {
            OnInvincibilityEvent?.Invoke();
            return;
        }

        CreateTextFeedback(damage);
        
        _damage = damage;
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        
        if (_currentHealth <= 0)
            IsDead = true;
        
        HitFeedBack();
    }

    private void HitFeedBack()
    {
        OnHit?.Invoke(_damage);
        
        if (IsDead)
            OnDead?.Invoke();
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => _maxHealth;

    public float GetNormalizeHealth() => _currentHealth / _maxHealth;

    public float GetPredictionNormalizeHealth(float damage)
    {
        return Mathf.Clamp(_currentHealth - damage / _maxHealth, 0f, 1f);
    }
    
    private void CreateTextFeedback(float damage)
    {
        float currentTime = Time.time; 
        if (_lastSpawnTime >= 0 && currentTime - _lastSpawnTime < 0.3f)
        {
            return;
        }

        _lastSpawnTime = currentTime; 

        var evt = SpawnEvents.TextCreateEvent;
        evt.poolType = PoolType.PopUpText;

        evt.value = damage < 0.1f ? "1" : damage.ToString("0");
    
        evt.position = (Vector2)transform.position + Random.insideUnitCircle;
        evt.fontColor = Color.red;
        evt.fontSize = Mathf.Clamp(0.4f, 1, _damage);
    
        _spawnEventChannel.RaiseEvent(evt);
    }

}
