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
        stateMachine.MovementData.JumpForceModifier = airborneData.JumpData.NormalJumpModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.JumpParameterHash);
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
        JumpingOrFalling();
    }

    public override void Exit()
    {
        jumpFinish = false;
        isJumping = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has salido del estado de SALTAR.");
    }

    private void FinishJump()
    {
        Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            jumpFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    
    protected override void Jump()
    {
        if(!isJumping)
        {
            float jumpForce = airborneData.BaseForceJump * (1 + stateMachine.MovementData.JumpForceModifier);
            //jumpForce = Mathf.Clamp(jumpForce, 0f, 10f); // Por si queremos poner un tope a la fuerza de salto.
            stateMachine.Player.RbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            Debug.Log(jumpForce);
        }        
    }

    private void JumpingOrFalling()
    {
        if (stateMachine.Player.RbPlayer.velocity.y < 5)
        {
            Debug.Log("Es un salto.");
            return;
        }
        else if(stateMachine.Player.RbPlayer.velocity.y >= 5)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            Debug.Log("Has pasado de salto a caída.");
        }
    }
}
