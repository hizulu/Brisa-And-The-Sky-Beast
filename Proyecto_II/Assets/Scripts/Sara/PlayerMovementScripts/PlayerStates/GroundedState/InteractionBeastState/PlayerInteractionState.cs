using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionState : PlayerGroundedState
{
    public PlayerInteractionState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.InteractionsParameterHash);
        //Debug.Log("Has entrado en estado de Interacción con la Bestia.");
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
        StopAnimation(stateMachine.Player.PlayerAnimationData.InteractionsParameterHash);
        //Debug.Log("Has salido del estado de Interacción con la Bestia.");
    }
}
