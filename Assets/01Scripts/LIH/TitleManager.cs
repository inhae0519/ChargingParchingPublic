using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{
    [Header("Text info")]
    [SerializeField] private float alphaValue = 0.6f;
    [SerializeField] private float time = 1.2f;
    [SerializeField] private TextMeshProUGUI startBtnText;
    
    [Space]
    
    [Header("Bgm info")]
    [SerializeField] private GameEventChannelSO eventChannelSo;
    [SerializeField] private SoundSO soundSO;
    
    
    [Header("Sound info")]
    [SerializeField] private SoundSO clickSound;

    [Header("Fade info")] 
    [SerializeField] private Image fadeInImage;
    [SerializeField] private float fadeTime;
    
    private void Awake()
    {
        startBtnText.DOFade(alphaValue, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InSine);
    }
    private void Start()
    {
        var evt = SoundEvents.PlayBGMEvent;
        evt.clipData = soundSO;
        eventChannelSo.RaiseEvent(evt);
    }
    public void PlayButtonSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = clickSound;
        eventChannelSo.RaiseEvent(evt);
    }
    
    public void TransitionNextScene(string nextScene)
    {
        fadeInImage.DOFade(1 , fadeTime).OnComplete(() =>
        {
            SceneManager.LoadScene(nextScene);
        });
    }
    
}
