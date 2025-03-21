using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyStateTemplate
{
    public EnemyPatrol(EnemyStateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyStateMachine.Enemy.matForDepuration.color = Color.blue; // Depuración TEMP
        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoEnterLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoFrameUpdateLogic();

        if (!enemyStateMachine.Enemy.doPatrol)
        {
            if (enemyStateMachine.Enemy.doIdle)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyIdleState);
            }
            else if (enemyStateMachine.Enemy.doChase)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyChaseState);
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoPhysiscsLogic();
    }

    public override void Exit()
    {
        base.Exit();
        enemyStateMachine.Enemy.matForDepuration.color = Color.gray; // Depuración TEMP
        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoExitLogic();
    }

    protected override void MoveEnemy()
    {

    }
}
