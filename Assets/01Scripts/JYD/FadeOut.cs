using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image _image;

    [SerializeField] private float fadeTime;
    
    void Start()
    {
        if(_image == null)
            _image = GetComponent<Image>();

        _image.color = new Color(_image.color.r , _image.color.g , _image.color.b , 1);
        
        _image.DOFade(0,fadeTime);
    }
}
