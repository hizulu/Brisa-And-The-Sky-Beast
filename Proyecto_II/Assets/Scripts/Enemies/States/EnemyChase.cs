using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyStateTemplate
{
    public EnemyChase(EnemyStateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.anim.SetBool("isMoving", false);
        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoPhysicsLogic();
    }
}
