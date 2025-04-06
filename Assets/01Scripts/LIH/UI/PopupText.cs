using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopupText : MonoBehaviour, IPoolable
{    
    [SerializeField] private PoolType _poolType;
    public PoolType Type => _poolType;
    public GameObject GameObject => gameObject;

    private Pool _myPool;

    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void ResetItem()
    {
        _tmpText.color = Color.white;
        transform.localScale = Vector3.one * 0.5f;
    }

    [SerializeField] private Vector2 _positionOffset;
    [SerializeField] private float _fadeInTime = 0.3f, _fadeOutTime = 0.7f;
    private TextMeshPro _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TextMeshPro>();
    }

    public void ShowPopupText(Vector3 position, string value, float fontSize, Color fontColor)
    {
        _tmpText.text = value;
        _tmpText.color = fontColor;

        float xPos = Random.Range(-_positionOffset.x, _positionOffset.x);
        float yPos = Random.Range(-_positionOffset.y, _positionOffset.y);

        transform.position = position + new Vector3(xPos, yPos);

        Sequence seq = DOTween.Sequence();
        seq.Append(_tmpText.DOFade(1f, _fadeInTime));
        seq.Join(transform.DOScale(Vector3.one * fontSize, 0.3f));
        seq.Append(_tmpText.DOFade(0f, _fadeOutTime));
        seq.AppendCallback(() => _myPool.Push(this));
    }
}
