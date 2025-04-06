using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraDistanceFeedback : Feedback
{
    [SerializeField] private GameEventChannelSO _camEventChannelSo;
    [SerializeField] private float _camSize;
    
    [SerializeField] private float _duration;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _returnSpeed = 2f;

    private Coroutine _coroutine;
    public override void CreateFeedback()
    {
        _coroutine = StartCoroutine(CamDistanceChangeRoutine());
    }

    private IEnumerator CamDistanceChangeRoutine()
    {
        var evt = CameraEvents.CamDistanceChangeEvent;
        evt.distance = _camSize;
        evt.speed = _speed;
        _camEventChannelSo.RaiseEvent(evt);

        yield return new WaitForSeconds(_duration);
        
        var evt2 = CameraEvents.CamDistanceResetEvent;
        evt2.speed = _returnSpeed;
        _camEventChannelSo.RaiseEvent(evt2);
        
        _coroutine = null;
    }
    
    public override void FinishFeedback()
    {
    }
}
