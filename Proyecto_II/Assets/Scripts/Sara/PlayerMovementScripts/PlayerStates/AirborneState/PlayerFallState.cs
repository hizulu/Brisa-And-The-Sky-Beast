using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirborneState
{
    public PlayerFallState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        Debug.Log("Has entrado en el estado de CAYENDO");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        Debug.Log("Has salido Del estado de CAYENDO");
    }

    protected override void ContactWithGround(Collider collider)
    {
        Debug.Log("Contacto con el suelo detectado");

        if (stateMachine.MovementData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);

            return;
        }
    }
}
