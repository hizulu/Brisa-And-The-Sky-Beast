using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerMovedState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.RunSpeedModif;
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.RunParameterHash);
        //Debug.Log("Has entrado en el estado de CORRER.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //if(stateMachine.MovementData.MovementInput == Vector2.zero)
        //    stateMachine.ChangeState(stateMachine.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.RunParameterHash);
        //Debug.Log("Has salido del estado de CORRER.");
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
}
