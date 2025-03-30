using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerStateMachine _stateMachine) : base(_stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        StartAnimation(stateMachine.Player.PlayerAnimationData.AirborneParameterHash);
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
        base .Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.AirborneParameterHash);
    }

    protected virtual void Jump()
    {
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual bool IsGrounded()
    {
        float radius = groundedData.GroundCheckDistance;
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;

        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);

        foreach (Collider collider in colliders)
        {

            if (collider.gameObject.layer == LayerMask.NameToLayer("Enviroment") && !collider.isTrigger)
            {
                //Debug.Log("Has tocado suelo");
                return true;
            }
        }
        //Debug.Log("No estás tocando suelo");
        return false;
    }

    protected override void ContactWithGround(Collider collider)
    {

    }

    protected virtual void MoveAirborne()
    {
        if (stateMachine.PreviousState is PlayerIdleState)
        {
            Debug.Log("El método de moverte en el aire se ejecuta");

            if (stateMachine.MovementData.MovementInput == Vector2.zero)
                return;

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 movementDirection = cameraForward * stateMachine.MovementData.MovementInput.y + Camera.main.transform.right * stateMachine.MovementData.MovementInput.x;
            movementDirection.Normalize();

            float airControlFactor = airborneData.AirControl;
            float airSpeed = airborneData.AirSpeed * airControlFactor;

            Vector3 newVelocity = new Vector3(movementDirection.x * airSpeed, stateMachine.Player.RbPlayer.velocity.y, movementDirection.z * airSpeed);
            stateMachine.Player.RbPlayer.velocity = newVelocity;

            Rotate(movementDirection);

            Debug.Log(stateMachine.Player.RbPlayer.velocity);
        }        
    }
}
