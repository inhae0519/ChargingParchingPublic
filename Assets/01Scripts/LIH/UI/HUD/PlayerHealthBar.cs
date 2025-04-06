using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerHealthBar : HudBarUI
{
    [SerializeField] private float _shakeValue;
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    [SerializeField] private TextMeshProUGUI _text;
    private Player _player;

    private Vector3 _defaultPos;
    
    private void Start()
    {
        _player = _playerManagerSo.Player;

        _player.GetEntityCompo<Health>().OnDead.AddListener(HandleDisableEvent);
        _player.GetEntityCompo<Health>().OnHit.AddListener(HandleFillEvent);
        
        
        _text.SetText($"{_player.GetEntityCompo<Health>().GetCurrentHealth():0} / {_player.GetEntityCompo<Health>().GetMaxHealth():0}");
        _defaultPos = transform.position;
    }

    protected override void HandleDisableEvent()
    {
        _text.SetText(
            $"{_player.GetEntityCompo<Health>().GetCurrentHealth():0} / {_player.GetEntityCompo<Health>().GetMaxHealth():0}");
        float fillAmount = _player.GetEntityCompo<Health>().GetNormalizeHealth();
        _fillImage.DOFillAmount(fillAmount, 0.3f);
    }

    protected override void HandleFillEvent(float damage)
    {
        transform.DOShakePosition(0.1f, _shakeValue).OnComplete(() => transform.position = _defaultPos);
        _text.SetText(
            $"{_player.GetEntityCompo<Health>().GetCurrentHealth():0} / {_player.GetEntityCompo<Health>().GetMaxHealth():0}");
        float fillAmount = _player.GetEntityCompo<Health>().GetNormalizeHealth();
        _fillImage.DOFillAmount(fillAmount, 0.3f);
    }
}
