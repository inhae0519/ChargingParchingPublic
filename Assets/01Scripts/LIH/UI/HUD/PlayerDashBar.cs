using System;
using DG.Tweening;
using UnityEngine;

public class PlayerDashBar : HudBarUI
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    private Player _player;

    private void Start()
    {
        _player = _playerManagerSo.Player;
        _player.GetPlayerCompo<PlayerMovement>()._dashCoolEvent.AddListener(HandleDashCoolBar);
    }

    private void OnDestroy()
    {
        _player.GetPlayerCompo<PlayerMovement>()._dashCoolEvent.RemoveListener(HandleDashCoolBar);
    }

    private void HandleDashCoolBar(float current, float cool)
    {
        float fill = current / cool;
        HandleFillEvent(1 - fill);
    }

    protected override void HandleDisableEvent()
    {
    }

    protected override void HandleFillEvent(float damage)
    {
        float fillAmount = damage;
        _fillImage.DOFillAmount(fillAmount, 0.1f);

    }
}
