using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pool/Item")]
public class PoolItemSO : ScriptableObject
{
    public PoolType poolType;
    public GameObject prefab;
    public int initCount;
}
