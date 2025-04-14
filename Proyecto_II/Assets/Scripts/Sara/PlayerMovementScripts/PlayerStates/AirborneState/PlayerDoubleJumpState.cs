using UnityEngine;

public class PlayerDoubleJumpState : PlayerAirborneState
{
    public PlayerDoubleJumpState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en el estado de DOBLE-SALTO");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Jump();
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Has salido del estado de DOBLE-SALTO");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DoubleJumpParameterHash);
    }

    protected override void Jump()
    {
        if(!isJumping)
        {
            // Este if es necesario para que, si está cayendo, la fuerza negativa en Y no contrarreste el impulso.
            if (stateMachine.PreviousState is PlayerFallState) // Asignamos la velocidad en Y en 0, para que se aplique bien la fuerza del doble salto.
                stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, 0f, stateMachine.Player.RbPlayer.velocity.z);

            float jumpForce = airborneData.BaseForceJump * (1 + airborneData.JumpData.DoubleJumpModif);
            //jumpForce = Mathf.Clamp(jumpForce, 0f, 10f); // Por si queremos poner un tope a la fuerza de salto.
            stateMachine.Player.RbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    protected override void FinishAnimation()
    {
        Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isJumping = false;
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }
}
