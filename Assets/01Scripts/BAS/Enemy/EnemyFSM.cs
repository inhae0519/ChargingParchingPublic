using UnityEngine;

public class EnemyFSM : MonoBehaviour,IEntityComponent//,IAfterInitable
{
    private Enemy _enemy;

    public EnemyStateSO CurrentState;

    private void Start()
    {
        CurrentState.OnEnter(_enemy);
    }

    public void Initialize(Entity entity)
    {
        _enemy = entity as Enemy;

    }

    public void SetState(EnemyStateSO state)
    {

        if(CurrentState !=null)
        {
            CurrentState.OnExit();
        }
        CurrentState = state;
        CurrentState.OnEnter(_enemy);
    }

    public void StateEnd()
    {
        Debug.LogWarning(CurrentState.StateName);
        CurrentState.DoExit();
    }

    private void Update()
    {
        CurrentState.Update();
    }

}
