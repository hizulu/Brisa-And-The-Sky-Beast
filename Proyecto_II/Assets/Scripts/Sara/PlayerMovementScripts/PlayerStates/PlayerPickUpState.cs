using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpState : PlayerMovementState
{
    public PlayerPickUpState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        EventsManager.TriggerNormalEvent("PickUpItem");
        StartAnimation(stateMachine.Player.PlayerAnimationData.PickUpParameterHash);
    }

    public override void Exit() 
    { 
        base.Exit(); 
        StopAnimation(stateMachine.Player.PlayerAnimationData.PickUpParameterHash);
    }
}
