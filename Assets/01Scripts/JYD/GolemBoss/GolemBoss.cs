using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;


public enum Pattern
{
    Bullet,
    Rock
}

[RequireComponent(typeof(CinemachineImpulseSource))]
public class GolemBoss : Entity
{
    [SerializeField] private CinemachineImpulseSource ImpulseSource;
    [SerializeField] private AnimationCurve handMoveCurve;
            
    [Space]
    [SerializeField] private GameEventChannelSO eventChannelSo;
    [SerializeField] private GameEventChannelSO soundEvent;
    
    [Space]
    [SerializeField] private SoundSO bgm;
    
    [SerializeField] private SoundSO golemImpactSound;
    [SerializeField] private SoundSO golemHitSound;
    [SerializeField] private SoundSO goelmDeadSound;
    
    private GolemBossBulletPattern _bulletPattern;
    public GolemBossBulletPattern BulletPattern => _bulletPattern;

    private GolemBossHandPattern _handPattern;
    public GolemBossHandPattern HandPattern => _handPattern;
    
    [SerializeField] private BehaviorGraphAgent _agent;
        
    protected override void Awake()
    {
        base.Awake();
        _handPattern = GetComponent<GolemBossHandPattern>();
        _bulletPattern = GetComponent<GolemBossBulletPattern>();
    }

    private void Start()
    {
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        
        var evt = SoundEvents.PlayBGMEvent;
        evt.clipData = bgm;
        soundEvent.RaiseEvent(evt);
    }
        
    private void ShakeCamera(float power) => ImpulseSource.GenerateImpulse(power);

    public float GetEasing(float startTime,  float endDistance , float speed)
    {
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / endDistance;
        float curvedFraction = handMoveCurve.Evaluate(Mathf.Clamp01(fractionOfJourney));
                
        return curvedFraction;
    }

    public void PlaySound(SoundSO so)
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = so;
        soundEvent.RaiseEvent(soundEvt);
    }
    
    public void PlayImpacts(Vector3 _pos)
    {
        ShakeCamera(2);
                
        var evt = SpawnEvents.SmokeParticleCreate;
        evt.position = _pos + new Vector3(0,-1.5f,0);
        evt.poolType = PoolType.ImpactParticle;
        eventChannelSo.RaiseEvent(evt);

        PlaySound(golemHitSound);
    }
        
    public void Dead()
    {
        _agent.enabled = false;
        PlaySound(goelmDeadSound);;
    }
}
