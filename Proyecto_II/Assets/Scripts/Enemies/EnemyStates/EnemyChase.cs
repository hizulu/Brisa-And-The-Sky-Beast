using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyStateTemplate
{
    public EnemyChase(EnemyStateMachine _stateMachine) : base(_stateMachine)
    {
    }
    public override void Enter() { }

    public override void Exit() { }

    public override void OnTriggerEnter(Collider collider) { }

    public override void OnTriggerExit(Collider collider) { }

    public override void UpdateLogic() { }

    public override void UpdatePhysics() { }
}
