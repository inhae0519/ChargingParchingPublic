using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField] private float hitStopDuration;
    [SerializeField] private float hitTime = 0.1f;
    private float originTime = 1;

    public void StartHitStop(float _damageAmount)
    {
        if (_damageAmount >= 10)
        {
            StartCoroutine(HitStopRoutine());
        }
    }
    
    
    IEnumerator HitStopRoutine()
    {
        Time.timeScale = hitTime;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1;
    }
    
    
}
