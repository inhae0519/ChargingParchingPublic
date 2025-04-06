using UnityEngine;

public class TrailRendererSizeHandle : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trail;

    [SerializeField] private float _maxWidth = 1.5f;
    [SerializeField] private float _maxWidthPower = 100f;
        
    public void SetWidth(float width)
    {
        //float t = Mathf.InverseLerp(0, _maxWidthPower, width);
        float lerp = Mathf.Lerp(0, _maxWidth, width/100) +0.02f;
        _trail.widthMultiplier = lerp;
    }
}
