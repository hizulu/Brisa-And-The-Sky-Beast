using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinalDeadState : PlayerDeathState
{
    public PlayerFinalDeadState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Has entrado en el estado de MUERTE FINAL");
        StartAnimation(stateMachine.Player.PlayerAnimationData.FinalDeadParameterHash);
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
        Debug.Log("Has salido del estado de MUERTE FINAL");
        StopAnimation(stateMachine.Player.PlayerAnimationData.FinalDeadParameterHash);
    }
}
