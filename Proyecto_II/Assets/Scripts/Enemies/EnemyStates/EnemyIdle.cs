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
        enemyStateMachine.Enemy.matForDepuration.color = Color.green; // Depuración TEMP
        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();

        if (!enemyStateMachine.Enemy.doIdle)
        {
            if (enemyStateMachine.Enemy.doPatrol)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyPatrolState);
            } else if (enemyStateMachine.Enemy.doChase)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyChaseState);
            }
        }
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoPhysiscsLogic();
    }

    public override void Exit() 
    {
        base.Exit();
        enemyStateMachine.Enemy.matForDepuration.color = Color.gray; // Depuración TEMP
        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoExitLogic();
    }
}
