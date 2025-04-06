using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerDashType
{
    MouseDir,
    InputDir,
}

public class Player : Entity
{
    public PlayerDashType dashType = PlayerDashType.InputDir;
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [field: SerializeField] public PlayerInputSO PlayerInput { get; set; }

    private Dictionary<Type, IPlayerCompo> _playerCompos;
    
    [Header("Sound")]
    [SerializeField] private GameEventChannelSO _soundChannelSo;
    [SerializeField] private SoundSO _hitSound;

    protected override void Awake()
    {
        base.Awake();
        _playerManagerSO.SetPlayer(this);
    }

    protected override void AfterInitialize()
    {
        base.AfterInitialize();
        
        _playerCompos = new Dictionary<Type, IPlayerCompo>();
        
        GetComponentsInChildren<IPlayerCompo>()
            .ToList()
            .ForEach(compo => _playerCompos.Add(compo.GetType(), compo));

        foreach (var compo in _playerCompos.Values)
        {
            compo.Initialize(this);
        }
    }

    public T GetPlayerCompo<T>() where T : IPlayerCompo
    {
        if(_playerCompos.TryGetValue(typeof(T), out IPlayerCompo compo))
        {
            return (T)compo;
        }
        return default;
    }


    public Vector2 LooDir => (PlayerInput.MousePos - (Vector2)transform.position).normalized;
    
    public void HitSoundPlay()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _hitSound;
        _soundChannelSo.RaiseEvent(evt);
    }
}
