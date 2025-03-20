using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateTemplate : IState
{
    protected EnemyStateMachine enemyStateMachine;

    public EnemyStateTemplate(EnemyStateMachine _enemyStateMachine)
    {
        enemyStateMachine = _enemyStateMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }

    public virtual void OnTriggerEnter(Collider collider) { }

    public virtual void OnTriggerExit(Collider collider) { }

    public virtual void UpdateLogic() { }

    public virtual void UpdatePhysics()
    {
        MoveEnemy();
    }

    protected virtual void MoveEnemy()
    {

    }
}
