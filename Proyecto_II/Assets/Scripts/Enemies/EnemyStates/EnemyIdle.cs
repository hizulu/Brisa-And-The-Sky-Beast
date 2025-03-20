using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStateTemplate
{
    public EnemyIdle(EnemyStateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Este es el script de EnemyIdle");
        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void OnTriggerEnter(Collider collider) { }

    public override void OnTriggerExit(Collider collider) { }

    public override void UpdateLogic() { }

    public override void UpdatePhysics() { }

    public override void Exit() { }

    protected override void MoveEnemy()
    {

    }
}
