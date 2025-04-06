using System;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _spawnEventChannelSo;
    [SerializeField] private PoolManagerSO _poolManager;

    private void Awake()
    {
        _spawnEventChannelSo.AddListener<TextCreate>(HandleCreate);
    }

    private void OnDestroy()
    {
        _spawnEventChannelSo.RemoveListener<TextCreate>(HandleCreate);
    }

    private void HandleCreate(TextCreate evt)
    {
        PopupText text = _poolManager.Pop(evt.poolType) as PopupText;
        text.ShowPopupText(evt.position, evt.value, evt.fontSize, evt.fontColor);
    }
}
