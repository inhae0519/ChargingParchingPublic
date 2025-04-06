using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerInputSO _playerInput;
    [SerializeField] private GameEventChannelSO _uiEventChannel;

    private void Awake()
    {
        _playerInput.OpenMenuEvent += HandleOpenMenu;
    }

    private void OnDestroy()
    {
        _playerInput.OpenMenuEvent -= HandleOpenMenu;
    }

    private void HandleOpenMenu()
    {
        _uiEventChannel.RaiseEvent(UIEvents.OpenMenu);
    }
}
