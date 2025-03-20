using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.AirborneParameterHash);
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
        base .Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.AirborneParameterHash);
    }

    protected virtual void Jump()
    {
        playerStateMachine.ChangeState(playerStateMachine.JumpState);
    }

    protected override void ContactWithGround(Collider collider)
    {

    }
}
