using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.LandParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.LandParameterHash);
    }
}
