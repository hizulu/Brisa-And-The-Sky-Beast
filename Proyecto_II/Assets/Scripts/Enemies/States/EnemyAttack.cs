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

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyAttackBaseInstance.DoPhysicsLogic();
    }
}
