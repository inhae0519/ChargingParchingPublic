using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCameraControl : MonoBehaviour, IPlayerCompo
{
    private Player _player;
    private PlayerWeaponController _playerWeaponController;
    
    [SerializeField] private GameEventChannelSO _cameraEventChannel;
    
    [Header("Charging Setting")]
    [SerializeField] private float _camDistanceMinValue = 4f;
    [SerializeField] private float _chargeShakeMin;
    [SerializeField] private float _chargeShakeMax;
    [SerializeField] private float _chargeZoomInPow;

    [Header("Fire Setting")]
    [SerializeField] private float _camShakeMaxValue = 50f;
    [SerializeField] private float _fireMinPower;
    [SerializeField] private float _fireMaxPower;

    private float _camDefaultDistance;
    private float _currentCamZoomInDel;
    
    public void Initialize(Player player)
    {
        _player = player;
        _playerWeaponController = player.GetPlayerCompo<PlayerWeaponController>();
    }

    private void Start()
    {
        _camDefaultDistance = FindAnyObjectByType<PlayerCamera>().DefaultDistance;
    }

    public void ChargingCamSetting(float chargingTime, float chargingValue)
    {
        if(_playerWeaponController.CurrentLevelIndex < 4)
            CamChargingDistanceChange(chargingTime,chargingValue);
        else
            CamChargingShake(chargingTime,chargingValue);
    }

    public void FireCamSetting(float power)
    {
        CamFireDistanceChange(power);
        CamFireShake(power);
    }

    public void CamReset()
    {
        var evt = CameraEvents.CamDistanceResetEvent;
        _cameraEventChannel.RaiseEvent(evt);
    }

    private void CamChargingDistanceChange(float chargingTime, float chargingValue)
    {
        var evt = CameraEvents.CamDistanceChangeEvent;
        float t = Mathf.InverseLerp(0, _playerWeaponController.levelSeconds[4], chargingTime);
        float inverseLerp = Mathf.Pow(t, _chargeZoomInPow);
        
        float distance = Mathf.Lerp(_camDefaultDistance, _camDistanceMinValue, inverseLerp);
        evt.distance = distance;
        evt.speed = chargingTime * _playerWeaponController.levelAdditiveValue[_playerWeaponController.CurrentLevelIndex];
        _cameraEventChannel.RaiseEvent(evt);
    }
    
    private void CamChargingShake(float chargingTime, float chargingValue)
    {
        
        var evt = CameraEvents.CamShakeEvent;
        evt.intensity = Random.Range(_chargeShakeMin, _chargeShakeMax);
        _cameraEventChannel.RaiseEvent(evt);
    }
    
    private void CamFireDistanceChange(float power)
    {
        var evt = CameraEvents.CamDistanceResetEvent;
        evt.speed = Mathf.Clamp(power, 1, 100);
        _cameraEventChannel.RaiseEvent(evt);
    }
    
    private void CamFireShake(float power)
    {
        var evt = CameraEvents.CamShakeEvent;
        
        float inverseLerp = Mathf.InverseLerp(0, _camShakeMaxValue, power);
        float intensity = Mathf.Lerp(_fireMinPower, _fireMaxPower, inverseLerp);

        evt.intensity = intensity;
        _cameraEventChannel.RaiseEvent(evt);
    }
}
