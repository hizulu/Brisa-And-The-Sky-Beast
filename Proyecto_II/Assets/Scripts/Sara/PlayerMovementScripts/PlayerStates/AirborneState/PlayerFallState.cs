using UnityEngine;

public class PlayerFallState : PlayerAirborneState
{
    public PlayerFallState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        //Debug.Log("Has entrado en el estado de CAYENDO");
    }

    public override void HandleInput()
    {
        base.HandleInput();

        //if (stateMachine.Player.PlayerInput.PlayerActions.Jump.triggered)
        //{
        //    if (maxNumDoubleJump < 1)
        //    {
        //        maxNumDoubleJump++;
        //        stateMachine.ChangeState(stateMachine.DoubleJumpState);
        //    }
        //}
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        LandInGround();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        //IncreaseFallSpeed();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        //Debug.Log("Has salido del estado de CAYENDO");
    }

    private void LandInGround()
    {
        if (IsGrounded())
        {
            //Debug.Log("Pasas a ATERRIZAR");
            stateMachine.ChangeState(stateMachine.LandState);
            ResetDoubleJump();
            return;
        }
    }    

    private float fallSpeed = 0f;
    private float gravityAcceleration = 9.8f;
    private float maxSpeed = 20f;
    private void IncreaseFallSpeed()
    {
        fallSpeed = Mathf.Min(fallSpeed + gravityAcceleration * Time.deltaTime, maxSpeed);
        stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, -fallSpeed, stateMachine.Player.RbPlayer.velocity.z);
    }
}
