using DG.Tweening;
using UnityEngine;


public class BossHealthBar : HudBarUI
{
    [SerializeField] private Entity _boss;
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    private Player _player;

    [SerializeField] private float yPos;
    [SerializeField] private float posDuration;
    private void Start()
    {
        _player = _playerManagerSo.Player;
        
        _boss.GetEntityCompo<Health>().OnDead.AddListener(HandleDisableEvent);
        _boss.GetEntityCompo<Health>().OnHit.AddListener(HandleFillEvent);
        //_player.GetPlayerCompo<PlayerWeaponController>().chargingEvent.AddListener(HandlePredictionEvent);

        (transform as RectTransform).DOAnchorPosY(yPos , posDuration);
    }

    protected override void HandleDisableEvent()
    {
        _canvasGroup.DOFade(0, 1);
    }

    protected override void HandleFillEvent(float damage)
    {
        float fillAmount = _boss.GetEntityCompo<Health>().GetNormalizeHealth();
        _fillImage.DOFillAmount(fillAmount, 0.3f);
        
        // float width = Mathf.Lerp(0, _predictionImage.rectTransform.sizeDelta.x, fillAmount);
        // _predictionImage.rectTransform.sizeDelta = new Vector2(width, _predictionImage.rectTransform.sizeDelta.y);
    }
}
