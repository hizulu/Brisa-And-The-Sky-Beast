using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyStateTemplate
{
    public EnemyAttack(EnemyStateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyStateMachine.Enemy.matForDepuration.color = Color.red; // Depuración TEMP
        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoEnterLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoFrameUpdateLogic();

        if (!enemyStateMachine.Enemy.doAttack)
        {
            if (enemyStateMachine.Enemy.doChase)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyChaseState);
            }
            else if (enemyStateMachine.Enemy.doRetreat)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.EnemyRetreatState);
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoPhysiscsLogic();
    }

    public override void Exit()
    {
        base.Exit();
        enemyStateMachine.Enemy.matForDepuration.color = Color.gray; // Depuración TEMP
        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoExitLogic();
    }
}
