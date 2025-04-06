using System;
using System.Collections;
using UnityEngine;

public class GolemHand : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private BoxCollider2D _collider2D;

    [SerializeField] private float damage;

    public void SetActiveCollider(float _activeTime)
    {
        StartCoroutine(ActiveRoutine(_activeTime));
    }

    private IEnumerator ActiveRoutine(float _activeTime)
    {
        _collider2D.enabled = true;
        yield return new WaitForSeconds(_activeTime);
        _collider2D.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((whatIsTarget & (1 << other.gameObject.layer)) != 0)
        {
            other.TryGetComponent<IDamageable>(out IDamageable health);
            health.ApplyDamage(damage);
        }
    }
}
