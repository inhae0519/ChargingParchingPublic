using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecondBossMinian : Enemy
{
    [SerializeField]
    private PlayerManagerSO _playerManager;
    private float _currentActiveTime = 0;
    [SerializeField]
    private float _runDistanc= 8,_ChaseDistance=9;

    [SerializeField]
    private EnemyStateSO _run, _chase,_attack;
    [SerializeField]
    private float _dashSpeed = 100;
    private LayerMask _whatisObstacle;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private bool _isFilpable = true;
    [SerializeField]
    private Transform _firePos;
    [SerializeField]
    private GameEventChannelSO _spawnChanel;
    [SerializeField]
    private float _attackCoolDown = 3f;
    [SerializeField]
    private AnimStateSO _resetParam;
    [SerializeField]
    private Animator _animaotr;
    private float _currentAttackCooldown = 0;
    //private StatModifierSO _dashSpeed;
    protected override void Awake()
    {
        base.Awake();
        var fsm = GetEntityCompo<EnemyFSM>();
        fsm.SetState(_run);
        _animaotr = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animaotr.SetTrigger(_resetParam.HashValue);
    }

    protected override void Update()
    {
        if (_isFilpable)
        {
            _spriteRenderer.flipX = _playerManager.PlayerTrm.position.x < transform.position.x;
        }
    }

    private void FixedUpdate()
    {
        if(_currentAttackCooldown >= _attackCoolDown)
        {
            _currentAttackCooldown = 0;
            AttackLop();
        }
        else
        {
            _currentAttackCooldown += Time.fixedDeltaTime;
        }
    }


    // 이 아래는 애니메이션 트리거 날먹
    public void ProjectileAttack()
    {
        Vector2 dir = (_playerManager.PlayerTrm.position - transform.position).normalized*2;

        var evt = SpawnEvents.BulletCreate;
        evt.position = _firePos.position;
        evt.dir =dir;
        evt._bulletType = PoolType.EnemyHomingBullet;
        evt.damage = 5;

        _spawnChanel.RaiseEvent(evt);

        //GetEntityCompo<EnemyMovement>().Knockback(dir.normalized * (_dashSpeed + dir.magnitude));
        _isFilpable = false;
        
        //GetComponentInChildren<Animator>(). SetBool("Attack", false);
    }
    public void DashAttackEnd()
    {
        //GetEntityCompo<EnemyMovement>().SetVelocity(Vector2.zero);
        _isFilpable = true;
    }

    //private IEnumerator AttackLoop()
    //{
    //    var fsm = GetEntityCompo<EnemyFSM>();
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(_attackCoolDown);
    //        fsm.SetState(_attack);
    //    }
    //}

    private void AttackLop()
    {
        var fsm = GetEntityCompo<EnemyFSM>();

            fsm.SetState(_attack);
    }

    public void StateEnd()
    {
        //GetEntityCompo<EnemyFSM>().StateEnd();

        var fsm = GetEntityCompo<EnemyFSM>();

        if (Vector2.Distance(transform.position, _playerManager.PlayerTrm.position) < _runDistanc&&
            (!Physics2D.Raycast(transform.position, _playerManager.PlayerTrm.position - transform.position, Vector2.Distance(_playerManager.PlayerTrm.position, transform.position), _whatisObstacle)))
        {
                fsm.SetState(_run);
        }
        else
        {
                fsm.SetState(_chase);
        }
        //_isFilpable = true;
    }
}
