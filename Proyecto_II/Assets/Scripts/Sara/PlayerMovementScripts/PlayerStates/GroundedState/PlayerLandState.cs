using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    private bool landFinish;

    public override void Enter()
    {
        landFinish = false;
        base.Enter();
        //Debug.Log("Has entrado en estado de ATERRIZAR");
        StartAnimation(stateMachine.Player.PlayerAnimationData.LandParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishLand();
    }

    public override void Exit()
    {
        landFinish = false;
        base.Exit();
        //Debug.Log("Has salido del estado de ATERRIZAR");
        StopAnimation(stateMachine.Player.PlayerAnimationData.LandParameterHash);
    }

    private void FinishLand()
    {
        Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aterrizaje") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            landFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
