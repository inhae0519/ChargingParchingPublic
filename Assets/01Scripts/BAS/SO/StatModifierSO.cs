using UnityEngine;
[CreateAssetMenu(fileName = "ModifilerSO", menuName = "SO/Stat/StatMod")]
public class StatModifierSO : ScriptableObject
{
    public StatSO TargetStat;
    public float Value;
}
