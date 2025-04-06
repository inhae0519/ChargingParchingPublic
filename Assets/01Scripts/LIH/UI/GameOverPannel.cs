using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPannel : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    
    [SerializeField] private string _currentScene;
    [SerializeField] private GameEventChannelSO _gameEventChannelSo;
    [SerializeField] private SoundSO _so;
    
    [SerializeField] private float _alphaTime;
    [SerializeField] private Animator animator;

    [SerializeField] private Image _image;
    
    private CanvasGroup _group;
    

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }
    
    public void OpenWindow()
    {
        gameObject.SetActive(true);
        _group.DOFade(1, _alphaTime).OnComplete(() =>
        {
            Time.timeScale = 0f;
            animator.SetTrigger("Die");
        });
    }

    public void ReStart()
    {
        _playerManagerSo.Player.PlayerInput.SetPlayerInput(true);
        Time.timeScale = 1f;
        _image.DOFade(1, 0.3f).OnComplete(()=>SceneManager.LoadScene(_currentScene));
    }

    public void Lobby()
    {
        _playerManagerSo.Player.PlayerInput.SetPlayerInput(true);
        Time.timeScale = 1f;
        _image.DOFade(1, 0.3f).OnComplete(()=>SceneManager.LoadScene("LobbyScene"));
    }
}
