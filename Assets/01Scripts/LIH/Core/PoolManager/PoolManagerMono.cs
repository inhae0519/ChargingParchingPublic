using UnityEngine;

public class PoolManagerMono : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _managerSO;

    private void Awake()
    {
        _managerSO.InitializePool(transform);
    }
}
