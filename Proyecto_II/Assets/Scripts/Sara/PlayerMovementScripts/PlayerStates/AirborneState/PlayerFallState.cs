using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Debug.Log("Has entrado en el estado de CAYENDO");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        LandInGround();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        IncreaseFallSpeed();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.FallParameterHash);
        Debug.Log("Has salido del estado de CAYENDO");
    }

    //protected override void ContactWithGround(Collider collider)
    //{
    //    Debug.Log("Contacto con el suelo detectado");

    //    if (stateMachine.MovementData.MovementInput == Vector2.zero)
    //    {
    //        stateMachine.ChangeState(stateMachine.LandState);
    //        Debug.Log("Deberías aterrizar");
    //        return;
    //    }
    //}

    private void LandInGround()
    {
        if (IsGrounded())
        {
            Debug.Log("Deberías aterrizar");
            stateMachine.ChangeState(stateMachine.LandState);
            return;
        }
    }

    private bool IsGrounded()
    {
        float radius = groundedData.GroundCheckDistance;
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;

        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") && !collider.isTrigger)
            {
                Debug.Log("Has tocado suelo");
                return true;
            }
        }
        return false;
    }

    private float fallSpeed = 0f;
    private float gravityAcceleration = 9.8f;
    private float maxSpeed = 20f;
    private void IncreaseFallSpeed()
    {
        fallSpeed = Mathf.Min(fallSpeed + gravityAcceleration * Time.deltaTime, maxSpeed);
        stateMachine.Player.RbPlayer.velocity = new Vector3(stateMachine.Player.RbPlayer.velocity.x, -fallSpeed, stateMachine.Player.RbPlayer.velocity.z);
        Debug.Log(fallSpeed);
    }
}
