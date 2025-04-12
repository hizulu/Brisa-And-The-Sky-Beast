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
        stateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.CrouchSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        //EventsManager.TriggerSpecialEvent<bool>("CrouchState", true);
        Debug.Log("Has entrado en el estado de AGACHARSE.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (stateMachine.MovementData.MovementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.IdleState);

        if(!stateMachine.Player.PlayerInput.PlayerActions.Crouch.IsPressed())
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        //EventsManager.TriggerSpecialEvent<bool>("CrouchState", false);
        Debug.Log("Has salido del estado de AGACHARSE.");
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
}
