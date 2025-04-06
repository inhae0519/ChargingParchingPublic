using UnityEngine;
using UnityEngine.UI;

public abstract class HudBarUI : MonoBehaviour
{
    [SerializeField] protected Image _fillImage;
    
    protected CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    protected abstract void HandleDisableEvent();
    protected abstract void HandleFillEvent(float damage);
}
