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
        stateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.WalkSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.WalkParameterHash);
        //Debug.Log("Has entrado en el estado de CAMINAR.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        audioManager.PlaySFX(audioManager.walk);
        //AudioManager.Instance.PlaySFX(AudioManager.Instance.walk);
    }

    public override void Exit()
    {
        base.Exit();
        audioManager.StopSFX();
        //AudioManager.Instance.StopSFX();
        StopAnimation(stateMachine.Player.PlayerAnimationData.WalkParameterHash);
        //Debug.Log("Has salido del estado de CAMINAR.");
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
}
