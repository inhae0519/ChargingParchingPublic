using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image _image;
    [SerializeField] private float fadeTime;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void FadeStart(string name)
    {
        _image.DOFade(1, fadeTime).OnComplete(() => SceneManager.LoadScene(name));
    }
}
