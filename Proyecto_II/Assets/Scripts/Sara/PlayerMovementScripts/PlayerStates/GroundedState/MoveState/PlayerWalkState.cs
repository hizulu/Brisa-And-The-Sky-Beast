using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerMovedState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        playerStateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.WalkSpeedModif;
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.WalkParameterHash);
        //Debug.Log("Has entrado en el estado de CAMINAR.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.WalkParameterHash);
        //Debug.Log("Has salido del estado de CAMINAR.");
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
}
