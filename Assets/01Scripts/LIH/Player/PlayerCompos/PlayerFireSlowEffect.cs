using System.Collections;
using UnityEngine;

public class PlayerFireSlowEffect : MonoBehaviour, IPlayerCompo
{
    [SerializeField] private float _slowValue;
    [SerializeField] private float _duration;
    [SerializeField] private float _slowPowerMin;

    private Player _player;
    private Coroutine _coroutine;
    
    public void Initialize(Player player)
    {
        _player = player;
        _player.GetPlayerCompo<PlayerWeaponController>().fireEvent.AddListener(HandleTimeSlow);
    }

    private void OnDestroy()
    {
        _player.GetPlayerCompo<PlayerWeaponController>().fireEvent.RemoveListener(HandleTimeSlow);
    }

    private void HandleTimeSlow(float power)
    {
        if(power <= _slowPowerMin)
            return;
        Slow();
    }
    private void Slow()
    {
        if (_coroutine != null)
            return;
        _coroutine = StartCoroutine(StartSlow());
    }

    private IEnumerator StartSlow()
    {
        Time.timeScale = _slowValue;
        yield return new WaitForSeconds(_duration);
        Time.timeScale = 1f;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}
