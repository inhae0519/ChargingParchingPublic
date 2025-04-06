using DG.Tweening;
using TMPro;
using UnityEngine;

public class ChargeText : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    [SerializeField] private float _yellowValue;
    [SerializeField] private float _redValue;
    [SerializeField] private float _textSpeed;
    
    private TextMeshProUGUI _text;
    private float _beforeCharging;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().chargingEvent.AddListener(SetChargePercent);
        _playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().fireEvent.AddListener(SetChargePercentInit);
        _playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().resetEvent.AddListener(ResetText);
    }

    private void OnDestroy()
    {
        _playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().chargingEvent.RemoveListener(SetChargePercent);
        _playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().fireEvent.RemoveListener(SetChargePercentInit);
        _playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().resetEvent.RemoveListener(ResetText);
    }

    public void SetChargePercent(float chargingTime, float chargingValue)
    {
        float yValue = Mathf.InverseLerp(0, _yellowValue, chargingValue);
        float rValue = Mathf.InverseLerp(_yellowValue, _redValue, chargingValue);

        _text.color = new Color(1, 1 - rValue, 1 - yValue);

        DOTween.To(() => _beforeCharging, f => _beforeCharging = f, chargingValue, _textSpeed);
        _text.SetText(_beforeCharging.ToString("0") + "%");
        _beforeCharging = chargingValue;
    }

    public void SetChargePercentInit(float power)
    {
        ResetText();
    }

    private void ResetText()
    {
        _text.DOColor(Color.white, 0.25f);
        _text.SetText("0%");
    }
}
