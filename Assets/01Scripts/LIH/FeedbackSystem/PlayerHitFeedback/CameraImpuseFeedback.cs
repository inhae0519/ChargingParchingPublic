using UnityEngine;

public class CameraImpuseFeedback : Feedback
{
    [SerializeField] private GameEventChannelSO _camEventChannelSo;
    [SerializeField] private float _intensity;

    private float _lastFeedbackTime = -1f; 

    public override void CreateFeedback()
    {
        float currentTime = Time.time;
        if (_lastFeedbackTime >= 0 && currentTime - _lastFeedbackTime < 0.7f)
        {
            return;
        }

        _lastFeedbackTime = currentTime; 

        var evt = CameraEvents.CamShakeEvent;
        evt.intensity = _intensity;
        _camEventChannelSo.RaiseEvent(evt);
    }

    public override void FinishFeedback()
    {
    }
}