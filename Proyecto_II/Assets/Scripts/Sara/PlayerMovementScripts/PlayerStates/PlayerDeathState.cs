using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerMovementState
{
    public PlayerDeathState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Has entrado en el estado de MUERTE");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DeathParameterHash);
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
        Debug.Log("Has salido del estado de MUERTE");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DeathParameterHash);
    }
}
