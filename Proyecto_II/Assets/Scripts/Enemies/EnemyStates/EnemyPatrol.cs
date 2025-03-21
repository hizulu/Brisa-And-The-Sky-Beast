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
        enemyStateMachine.Enemy.matForDepuration.color = Color.blue;
        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoEnterLogic();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemyStateMachine.Enemy.EnemyPatrolBaseInstance.DoFrameUpdateLogic();
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
