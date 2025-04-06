using System;
using UnityEngine;

public abstract class EnemyStateSO : ScriptableObject,ICloneable
{
    public string StateName;

    public AnimStateSO AnimState;

    protected Enemy _enemy;

    public EnemyStateSO NextState;

    [SerializeField]
    protected PlayerManagerSO _playerSO;
    [SerializeField]
    protected StatModifierSO _statModifier;

    public abstract void Update();

    public abstract void OnEnter(Entity entity);

    public abstract void OnExit();

    public virtual void DoExit()
    {
        _enemy.GetEntityCompo<EnemyFSM>().SetState(NextState);
    }

    public object Clone()
    {
        return Instantiate(this);
    }
}
