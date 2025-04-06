using System;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _target;
    
    [SerializeField] private GameEventChannelSO _cameraChannel;
    [SerializeField] private float _distanceChangeRate = 1.5f, _distanceThreshold = 0.25f, _shakeThreshold = 0.1f;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    [SerializeField] private Transform Boss;
    
    private bool _isChangeComplete;

    private CinemachineCamera _vCam;
    private CinemachinePositionComposer _composer;

    private float _targetDistance;
    private float _camDistanceSpeed;
    private float _defaultYoffset;
    
    public float DefaultDistance { get; private set; }

    private void Awake()
    {
        _vCam = GetComponent<CinemachineCamera>();
        _composer = GetComponent<CinemachinePositionComposer>();
        _defaultYoffset = _composer.TargetOffset.y;
        
        _cameraChannel.AddListener<CamDistanceChange>(HandleCamDistanceChange);
        _cameraChannel.AddListener<CamShake>(HandleCamShake);
        _cameraChannel.AddListener<CamDistanceReset>(HandleCamDistanceReset);
        _cameraChannel.AddListener<CamOffsetChange>(HandleCamOffset);
        
        _targetDistance = _vCam.Lens.OrthographicSize;
        DefaultDistance = _vCam.Lens.OrthographicSize;
    }
    
    private void OnDestroy()
    {
        _cameraChannel.RemoveListener<CamDistanceChange>(HandleCamDistanceChange);
        _cameraChannel.RemoveListener<CamShake>(HandleCamShake);
        _cameraChannel.RemoveListener<CamDistanceReset>(HandleCamDistanceReset);
        _cameraChannel.RemoveListener<CamOffsetChange>(HandleCamOffset);
    }

    private void HandleCamOffset(CamOffsetChange evt)
    {
        Vector2 dir = evt.targetPos - evt.postion;

        float x = Mathf.Clamp(dir.x, -evt.radius, evt.radius);
        float y = Mathf.Clamp(dir.y, -evt.radius, evt.radius);

        DOTween.To(() => _composer.TargetOffset.x, f =>  _composer.TargetOffset.x = f,
            Mathf.Abs(x), 0.2f);
        DOTween.To(() => _composer.TargetOffset.y, f => _composer.TargetOffset.y = f,
            y, 0.2f);
    }

    private void HandleCamShake(CamShake evt)
    {
        _impulseSource.DefaultVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        _impulseSource.GenerateImpulse(evt.intensity * _shakeThreshold);
    }

    private void HandleCamDistanceChange(CamDistanceChange evt)
    {
        _targetDistance = evt.distance;
        _isChangeComplete = false;
        _camDistanceSpeed = evt.speed;
    }
    
    private void HandleCamDistanceReset(CamDistanceReset evt)
    {
        _targetDistance = DefaultDistance;
        _isChangeComplete = false;
        _camDistanceSpeed = evt.speed;
    }

    private void Update()
    {
        UpdateDefault();
        UpdateCameraDistance();
    }

    private void UpdateDefault()
    {
        if (_target == null || _player == null)
            return;
        
        float distance = Vector3.Distance(_target.position, _player.position);
        _targetDistance = Mathf.Clamp(distance, 7, 13);
        _isChangeComplete = false;
    }

    private void UpdateCameraDistance()
    {
        float currentDistance = _vCam.Lens.OrthographicSize;
        if (Mathf.Abs(currentDistance - _targetDistance) < _distanceThreshold)
        {
            _vCam.Lens.OrthographicSize = _targetDistance;
            _isChangeComplete = true;
        }
        else
        {
            _vCam.Lens.OrthographicSize = Mathf.Lerp(
                _vCam.Lens.OrthographicSize,
                _targetDistance,
                _distanceChangeRate * Time.deltaTime * _camDistanceSpeed);
        }
    }
    
    public async void ChangeTarget()
    {
        Transform origin = _vCam.Follow;
        
        _vCam.Follow = Boss;
        await Task.Delay(8000);
        _vCam.Follow = origin;
    }
}
