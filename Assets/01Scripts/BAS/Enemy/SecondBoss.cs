using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SecondBoss : Enemy
{
    [SerializeField]
    private PlayerManagerSO _playerManager;
    private float _currentActiveTime = 0;
    [SerializeField]
    private float _activeDegree = 5,_activingTime=8;

    [SerializeField]
    private EnemyStateSO _idle, _battlebeginner,_deadState;
    [SerializeField]
    private float _dashSpeed = 100,_activeDistance=1f;
    [SerializeField]
    private LayerMask _whatisObstacle;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private bool _isFilpable=true;
    [SerializeField]
    private StatSO _healthSO;

    public UnityEvent PostPage2;
    public UnityEvent Page2Start;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private bool _isHasSummon = true;

    [SerializeField]
    private Transform _firePos;
    [SerializeField]
    private GameEventChannelSO _spawnChanel;

    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private AnimStateSO _deadParam;
    [SerializeField]
    private GameObject EnemyMom;
    
    //private StatModifierSO _dashSpeed;
    protected override void Awake()
    {   
        base.Awake();
        var fsm = GetEntityCompo<EnemyFSM>();
        //fsm.enabled = false;
        fsm.SetState(_idle);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(Mathf.Abs(Vector2.SignedAngle((_playerManager.PlayerTrm.position - transform.position),
            (_playerManager.PlayerTrm.position - Camera.main.ScreenToWorldPoint(Mouse.current.position.value)))) < _activeDegree
            || Vector2.Distance(transform.position,_playerManager.PlayerTrm.position)<_activeDistance)
        {
           // if(!Physics2D.Raycast(transform.position,_playerManager.PlayerTrm.position - transform.position, Vector2.Distance(_playerManager.PlayerTrm.position, transform.position),_whatisObstacle))
            _currentActiveTime =_activingTime;
        }

        if (_currentActiveTime > 0)
        {
            _currentActiveTime -= Time.deltaTime;

            var fsm = GetEntityCompo<EnemyFSM>();

            //if(!fsm.enabled)
            if(fsm.CurrentState == _idle)
            {
                fsm.SetState(_battlebeginner);
                //fsm.enabled = true;
                EnemyMom.SetActive(true);
            }
            _anim.SetFloat("Battle", 1, 0.7f, 0.08f);
        }
        else
        {
            var fsm = GetEntityCompo<EnemyFSM>();
            if (fsm.CurrentState != _idle)
            {
                fsm.SetState(_idle);
                EnemyMom.SetActive(false);
                //fsm.enabled = false;
            }
            _anim.SetFloat("Battle", 0, 0.7f, 0.08f);
        }

        if(_isFilpable)
        {
            _spriteRenderer.flipX = _playerManager.PlayerTrm.position.x < transform.position.x;
        }
    }


    private void OnPostPage2()
    {
        PostPage2?.Invoke();
        
    }

    public void Page2StartEventExcuter()
    {
        Page2Start?.Invoke();
    }

    // 이 아래는 애니메이션 트리거 날먹
    public void DashAttack()
    {
        Vector2 dir = (_playerManager.PlayerTrm.position - transform.position);
        GetEntityCompo<EnemyMovement>().Knockback(dir.normalized*(_dashSpeed + dir.magnitude));
        _isFilpable = false;
    }
    public void DashAttackEnd()
    {
        GetEntityCompo<EnemyMovement>().SetVelocity(Vector2.zero);
        _isFilpable = true;
    }
    public void StateEnd()
    {
        GetEntityCompo<EnemyFSM>().StateEnd();
        //_isFilpable = true;
    }

    //이 아래는 이벤트 날먹...
    public void IsPage2(float damage)
    {
        if(_isHasSummon)
        {
            _isHasSummon = false;
            if (GetEntityCompo<Health>().GetCurrentHealth() <= GetEntityCompo<EntityStat>().GetStat(_healthSO).BaseValue / 2)
            {
               // OnPostPage2();
               //EnemyMom.SetActive(true);
            }
        }

    }

    public void ProjectileAttack()
    {
        Vector2 dir = (_playerManager.PlayerTrm.position - transform.position).normalized * 2;

        _firePos.up = dir.normalized;
        
        var evt = SpawnEvents.BulletCreate;
        evt.position = _firePos.position;
        evt.dir = dir;
        evt._bulletType = PoolType.EnemyFastBullet;
        evt.damage = 5;

        _spawnChanel.RaiseEvent(evt);

        //GetEntityCompo<EnemyMovement>().Knockback(dir.normalized * (_dashSpeed + dir.magnitude));
        _isFilpable = false;
    }

    public void Page2Anim()
    {
        _anim.SetTrigger("Page2");
    }

    [ContextMenu("testDamage")]
    public void TestInflic()
    {
        GetEntityCompo<Health>().ApplyDamage(600);
    }

    public void SetPortal()
    {
        portal.gameObject.SetActive(true);
    }

    public Animator Animator; //언년이 이거 써놨노... 아가...
    [SerializeField] private GameEventChannelSO _channelSo;
    [SerializeField] private EnemyMovement _enemyMovement;
    
    public void SetDead()
    {
        //Animator.enabled = false;
        _enemyMovement.enabled = false;
        enabled = false;
        
        for (int i = 0; i < 5; i++)
        {
            var ex = SpawnEvents.ExplosionCreate;
            ex.position = transform.position;
            ex.poolType = PoolType.ExplosionParticle;
            
            _channelSo.RaiseEvent(ex);
        }
    }

    public void DeadAnim()
    {
        var fsm = GetEntityCompo<EnemyFSM>();
        fsm.SetState(_deadState);
        _anim.SetTrigger(_deadParam.HashValue);
    }
}
