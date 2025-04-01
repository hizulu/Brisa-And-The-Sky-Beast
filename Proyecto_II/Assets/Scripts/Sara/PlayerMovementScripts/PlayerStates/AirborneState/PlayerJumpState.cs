using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    protected bool jumpFinish;
    private bool isJumping = false;
    protected bool doubleJump;

    private float jumpTimeElapsed;
    private float minTimeBeforeDoubleJump = 0.1f;

    public override void Enter()
    {
        jumpFinish = false;
        doubleJump = false;
        jumpTimeElapsed = 0f;
        stateMachine.MovementData.JumpForceModifier = 0f;
        base.Enter();
        //Vector3 velocity = stateMachine.Player.RbPlayer.velocity;
        //float maxAirSpeed = 5f;

        //Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
        //if (horizontalVelocity.magnitude > maxAirSpeed)
        //{
        //    horizontalVelocity = horizontalVelocity.normalized * maxAirSpeed;
        //}

        //stateMachine.Player.RbPlayer.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.y);
        StartAnimation(stateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has entrado en el estado de SALTAR.");
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpTimeElapsed > minTimeBeforeDoubleJump && stateMachine.Player.PlayerInput.PlayerActions.Jump.WasPressedThisFrame() && !doubleJump)
        {
            doubleJump = true;
            stateMachine.ChangeState(stateMachine.DoubleJumpState);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        jumpTimeElapsed += Time.deltaTime;
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
        StopAnimation(stateMachine.Player.PlayerAnimationData.JumpParameterHash);
        //Debug.Log("Has salido del estado de SALTAR.");
    }

    protected virtual void FinishJump()
    {
        Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            jumpFinish = true;
            stateMachine.ChangeState(stateMachine.FallState);
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
        }        
    }
}
