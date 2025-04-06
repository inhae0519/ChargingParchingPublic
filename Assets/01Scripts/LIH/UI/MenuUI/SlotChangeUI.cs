using System;
using UnityEngine;

public class SlotChangeUI : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;

    [SerializeField] private Transform _slot1Check;
    [SerializeField] private Transform _slot2Check;

    private void Start()
    {
        _playerManagerSo.Player.PlayerInput.SlotChangeEvent += HandleSlotChange;
    }

    private void OnDestroy()
    {
        _playerManagerSo.Player.PlayerInput.SlotChangeEvent -= HandleSlotChange;
    }

    private void HandleSlotChange(int index)
    {
        if(_playerManagerSo.Player.GetPlayerCompo<PlayerWeaponController>().IsChargingStart)
            return;
        
        if (index == 0)
        {
            _slot1Check.gameObject.SetActive(true);
            _slot2Check.gameObject.SetActive(false);
        }
        else
        {
            _slot1Check.gameObject.SetActive(false);
            _slot2Check.gameObject.SetActive(true);
        }
    }
}
