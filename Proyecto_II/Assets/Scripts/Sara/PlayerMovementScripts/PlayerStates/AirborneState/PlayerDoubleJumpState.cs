using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerJumpState
{
    public PlayerDoubleJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //doubleJump = false;
        Debug.Log("Has entrado en el estado de DOBLE-SALTO");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
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
        Debug.Log("Has salido del estado de DOBLE-SALTO");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
    }

    protected override void FinishJump()
    {
        Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //jumpFinish = true;
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }
}
