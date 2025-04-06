using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerCompo
{
    private Player _player;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private readonly int _moveHash = Animator.StringToHash("Move");
    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _deadHash = Animator.StringToHash("Dead");
    
    
    [SerializeField] private GameObject gun;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerWeaponController _weaponController;
    
    private bool isDead = false;
        
    public void Initialize(Player player)
    {
        _player = player;
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isDead)return;
        
        if (_rigidbody2D.linearVelocity.normalized.sqrMagnitude <= 0)
            HandleMoveAnim(false);
        else
            HandleMoveAnim(true);
    }

    private void HandleMoveAnim(bool isMove)
    {
        _animator.SetBool(_moveHash, isMove);
        _animator.SetBool(_idleHash, !isMove);
    }

    public void PlayDeadAnim()
    {
        isDead = true;
        
        _animator.SetBool(_idleHash, false);
        _animator.SetBool(_moveHash, false);

        _animator.SetBool(_deadHash , true);

    }
    
}
