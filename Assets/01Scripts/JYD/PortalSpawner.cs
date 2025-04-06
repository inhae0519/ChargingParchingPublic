using DG.Tweening;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private Portal portal;
    
    [Range(1, 10)] [SerializeField] private float delayTime;
    [Range(1, 10)] [SerializeField] private float spawnDuration;
    
    public void PortalSpawn()
    {
        DOVirtual.DelayedCall(delayTime , () =>
        {
            portal.gameObject.SetActive(true);
            portal.Spawn(spawnDuration);
        });
    }
    
    
    
}
