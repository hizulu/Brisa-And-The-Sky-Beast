using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRetreat : EnemyStateTemplate
{
    public EnemyRetreat(EnemyStateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemyStateMachine.Enemy.matForDepuration.color = Color.magenta; // Depuración temporal
        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.matForDepuration.color = Color.gray; // Depuración temporal
        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoFrameUpdateLogic();

        if (!enemyStateMachine.Enemy.doRetreat)
        {
            if (enemyStateMachine.Enemy.doChase)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyChaseState);
            }
            else if (enemyStateMachine.Enemy.doIdle)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyIdleState);
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoPhysicsLogic();
    }
}
