using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerMovedState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        playerStateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.CrouchSpeedModif;
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        Debug.Log("Has entrado en el estado de AGACHARSE.");
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        Debug.Log("Has salido del estado de AGACHARSE.");
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
}
