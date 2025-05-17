using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDismountBeastState : PlayerAirborneState
{
    public PlayerDismountBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.DismountBeastParameterHash);
        Debug.Log("Has entrado al estado de DESMONTAR DE LA BESTIA");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.DismountBeastParameterHash);        
        Debug.Log("Has salido del estado de DESMONTAR DE LA BESTIA");
    }

    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("DismountBeast") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }
}
