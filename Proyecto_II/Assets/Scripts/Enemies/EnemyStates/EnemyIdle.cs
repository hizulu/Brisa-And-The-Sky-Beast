using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStateTemplate
{
    public EnemyIdle(Enemy _enemy, EnemyStateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void Exit() { }

    public override void OnTriggerEnter(Collider collider) { }

    public override void OnTriggerExit(Collider collider) { }

    public override void UpdateLogic() { }

    public override void UpdatePhysics() { }
    protected override void MoveEnemy()
    {

    }
}
