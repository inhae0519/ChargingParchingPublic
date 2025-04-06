using UnityEngine;

public class HomingBullet : EnemyBullet
{
    [SerializeField]
    private PlayerManagerSO _playerManager;
    [SerializeField]
    private float _homingPower = 1f;
    
    
    
    private void FixedUpdate()
    {
        _rigidbody2D.AddForce((_playerManager.PlayerTrm.position - transform.position).normalized * _homingPower, ForceMode2D.Impulse);
        transform.up = _rigidbody2D.linearVelocity.normalized;
    }
}
