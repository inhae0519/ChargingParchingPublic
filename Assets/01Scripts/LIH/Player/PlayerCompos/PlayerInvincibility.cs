using System.Collections;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour, IPlayerCompo
{
    [SerializeField] private float _slowValue;
    [SerializeField] private float _duration;
    
    private Player _player;
    private Coroutine _coroutine;
    public void Initialize(Player player)
    {
        _player = player;
    }

    public void InvincibilitySlow()
    {
        if (_coroutine != null)
            return;
        _coroutine = StartCoroutine(StartSlow());
    }

    private IEnumerator StartSlow()
    {
        Time.timeScale = _slowValue;
        yield return new WaitForSeconds(_duration);
        Time.timeScale = 1;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}
