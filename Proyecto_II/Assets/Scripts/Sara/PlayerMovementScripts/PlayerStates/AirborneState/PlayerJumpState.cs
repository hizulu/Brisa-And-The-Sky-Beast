using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    private bool jumpFinish;
    private bool isJumping = false;

    public override void Enter()
    {
        jumpFinish = false;
        playerStateMachine.MovementData.JumpForceModifier = airborneData.JumpData.NormalJumpModif;
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has entrado en el estado de SALTAR.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishJump();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Jump();
    }

    public override void Exit()
    {
        jumpFinish = false;
        isJumping = false;
        base.Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has salido del estado de SALTAR.");
    }

    private void FinishJump()
    {
        Animator animator = playerStateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            jumpFinish = true;
            playerStateMachine.ChangeState(playerStateMachine.IdleState);
        }
    }
    
    protected override void Jump()
    {
        if(!isJumping)
        {
            float jumpForce = 5f;

            // MIRAR POR QUÉ NO FUNCIONA CON BASEFORCEJUMP.
            //float jumpForce = airborneData.BaseForceJump * (1 + airborneData.JumpData.NormalJumpModif);
            //jumpForce = Mathf.Clamp(jumpForce, 0f, 10f);

            playerStateMachine.Player.RbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }        
    }
}
