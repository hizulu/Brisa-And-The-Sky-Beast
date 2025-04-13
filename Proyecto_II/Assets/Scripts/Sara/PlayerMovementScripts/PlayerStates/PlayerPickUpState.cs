using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpState : PlayerMovementState
{
    public PlayerPickUpState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    private bool pickUpFinish;

    public override void Enter()
    {
        pickUpFinish = false;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.PickUpParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishPickUp();
    }

    public override void Exit() 
    { 
        base.Exit(); 
        StopAnimation(stateMachine.Player.PlayerAnimationData.PickUpParameterHash);
    }

    private void FinishPickUp()
    {
        Animator animator = stateMachine.Player.AnimPlayer;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickUp_Brisa") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            pickUpFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
