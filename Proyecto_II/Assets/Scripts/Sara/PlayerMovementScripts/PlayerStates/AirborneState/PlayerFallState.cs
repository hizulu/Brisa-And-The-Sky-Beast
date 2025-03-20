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
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.FallParameterHash);
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
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.FallParameterHash);
        Debug.Log("Has salido Del estado de CAYENDO");
    }

    protected override void ContactWithGround(Collider collider)
    {
        Debug.Log("Contacto con el suelo detectado");

        if (playerStateMachine.MovementData.MovementInput == Vector2.zero)
        {
            playerStateMachine.ChangeState(playerStateMachine.IdleState);

            return;
        }
    }
}
