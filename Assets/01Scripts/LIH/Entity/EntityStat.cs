using System.Linq;
using UnityEngine;

public class EntityStat : MonoBehaviour, IEntityComponent
{
    [SerializeField] private StatOverride[] _statOverrides;

    private StatSO[] _stats;

    private Entity _entity;
    
    public void Initialize(Entity entity)
    {
        _entity = entity;
        _stats = _statOverrides.Select(x => x.CreateStat()).ToArray();
    }

    public StatSO GetStat(StatSO stat)
    {
        StatSO findStat = _stats.FirstOrDefault(x => x.statName == stat.statName);
        Debug.Assert(findStat != null, $"stat is null : {stat.statName}");
        return findStat;
    }

    public bool TryGetStat(StatSO stat, out StatSO outStat)
    {
        outStat = _stats.FirstOrDefault(x => x.statName == stat.statName);
        return outStat != null;
    }

    public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;

    public float GetBaseValue(StatSO stat) => GetStat(stat).BaseValue;

    public float IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;

    public void AddModifier(StatSO stat, object key, float value) => GetStat(stat).AddModifier(key, value);
    
    public void RemoveModifier(StatSO stat, object key) => GetStat(stat).RemoveModifier(key);

    public void ClearAllModifiers()
    {
        foreach (StatSO stat in _stats)
        {
            stat.ClearModifier();
        }
    }
}
