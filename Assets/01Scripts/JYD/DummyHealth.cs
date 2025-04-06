using UnityEngine;
using UnityEngine.Events;

public class DummyHealth : MonoBehaviour ,IDamageable
{
    public float health;
    public UnityEvent dieEvent; 
    
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            dieEvent?.Invoke();
        }
    }

    public void SetHealth(float _health)
    {
        health = _health;
    }
}
