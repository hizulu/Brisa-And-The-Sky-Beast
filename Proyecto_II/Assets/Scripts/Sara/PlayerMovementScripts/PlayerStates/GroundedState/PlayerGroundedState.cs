using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE SCRIPT: PlayerGroundedState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovementState
 * VERSIÓN: 1.0.
 */
public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
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
        StopAnimation(stateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started += JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.started += CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started += AttackStart;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.Player.PlayerInput.PlayerActions.Jump.started -= JumpStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.started -= CrouchStarted;
        stateMachine.Player.PlayerInput.PlayerActions.Attack.started -= AttackStart;
    }

    protected virtual void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected virtual void OnStop()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void CrouchStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.CrouchState);
    }

    protected virtual void AttackStart(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.ComboAttack);
    }

    protected override void NoContactWithGround(Collider collider)
    {
        Debug.Log("MIERDA");

        if (IsGrounded())
            return;

        stateMachine.ChangeState(stateMachine.FallState);
    }

    private bool IsGrounded()
    {
        Debug.Log("IsGrounded() is being called"); // Confirmamos que la función se llama

        RaycastHit hit;
        Vector3 rayOrigin = stateMachine.Player.RbPlayer.position;

        Debug.DrawRay(rayOrigin, Vector3.down * groundedData.GroundCheckDistance, Color.red, 0.1f);
        Debug.Log("Disparando Raycast desde: " + rayOrigin);

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundedData.GroundCheckDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            if (stateMachine.Player.LayerData.IsGroundLayer(hit.collider.gameObject.layer))
            {
                Debug.Log("We are grounded.");
                return true;
            }
            else
            {
                Debug.Log("Layer is not Ground.");
            }
        }
        else
        {
            Debug.Log("Raycast NO ha detectado suelo.");
        }

        return false;
    }
}
