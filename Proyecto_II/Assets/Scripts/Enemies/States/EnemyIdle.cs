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

        enemyStateMachine.Enemy.anim.SetBool("isMoving", false);
        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyIdleBaseInstance.DoPhysicsLogic();
    }
}
