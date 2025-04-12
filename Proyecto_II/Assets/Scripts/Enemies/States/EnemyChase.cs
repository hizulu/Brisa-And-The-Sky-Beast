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

        enemyStateMachine.Enemy.matForDepuration.color = Color.yellow; // Depuración temporal
        enemyStateMachine.Enemy.anim.SetBool("isMoving", true);
        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoEnterLogic();
    }

    public override void Exit()
    {
        base.Exit();

        enemyStateMachine.Enemy.matForDepuration.color = Color.gray; // Depuración temporal
        enemyStateMachine.Enemy.anim.SetBool("isMoving", false);
        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoExitLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoFrameUpdateLogic();

        //if (!enemyStateMachine.Enemy.doChase)
        //{
        //    if (enemyStateMachine.Enemy.doAttack)
        //    {
        //        enemyStateMachine.ChangeState(enemyStateMachine.EnemyAttackState);
        //    }
        //    else if (enemyStateMachine.Enemy.doPatrol)
        //    {
        //        enemyStateMachine.ChangeState(enemyStateMachine.EnemyPatrolState);
        //    }
        //}
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        enemyStateMachine.Enemy.EnemyChaseBaseInstance.DoPhysicsLogic();
    }
}
