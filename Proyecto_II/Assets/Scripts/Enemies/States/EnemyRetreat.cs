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

        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyRetreatBaseInstance.DoPhysicsLogic();
    }
}
