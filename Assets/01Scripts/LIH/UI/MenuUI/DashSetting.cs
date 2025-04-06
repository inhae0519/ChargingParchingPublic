using System;
using UnityEngine;
using UnityEngine.UI;

public class DashSetting : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    [SerializeField] private Image _check;
    private bool _isMouseDash = false;
    
    private void Start()
    {
        int savedDashType = PlayerPrefs.GetInt("DashSetting", (int)PlayerDashType.InputDir);
            
        _playerManagerSo.Player.dashType = (PlayerDashType)savedDashType;
        _isMouseDash = _playerManagerSo.Player.dashType == PlayerDashType.MouseDir;
        _check.gameObject.SetActive(_isMouseDash);
    }

    public void Select()
    {
        _isMouseDash = !_isMouseDash;
        _check.gameObject.SetActive(_isMouseDash);
        
        if (_isMouseDash)
        {
            _playerManagerSo.Player.dashType = PlayerDashType.MouseDir;
        }
        else
        {
            _playerManagerSo.Player.dashType = PlayerDashType.InputDir;
        }
        
        PlayerPrefs.SetInt("DashSetting", (int)_playerManagerSo.Player.dashType);
        
    }
}
