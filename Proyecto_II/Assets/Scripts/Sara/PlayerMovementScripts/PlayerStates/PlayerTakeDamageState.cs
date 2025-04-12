using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamageState : PlayerMovementState
{
    public PlayerTakeDamageState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    private bool takeDamageFinish;

    public override void Enter()
    {
        takeDamageFinish = false;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.TakeDamageParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishTakeDamage();
    }

    public override void Exit()
    { 
        base.Exit(); 
        StopAnimation(stateMachine.Player.PlayerAnimationData.TakeDamageParameterHash);
    }

    private void FinishTakeDamage()
    {
        Animator animator = stateMachine.Player.AnimPlayer;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            takeDamageFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
