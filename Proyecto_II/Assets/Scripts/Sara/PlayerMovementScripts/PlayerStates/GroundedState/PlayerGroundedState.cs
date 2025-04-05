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
        stateMachine.Player.PlayerInput.PlayerActions.Run.performed += RunStarted;
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
        if (stateMachine.Player.PlayerInput.PlayerActions.Crouch.IsPressed())
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
            return;
        }

        if (stateMachine.Player.PlayerInput.PlayerActions.Run.IsPressed())
            stateMachine.ChangeState(stateMachine.RunState);
        else
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected virtual void OnStop()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected virtual void RunStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.RunState);
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
        // Solo cambiar a Attack01 si no estamos en medio de un combo o ataque
        if (!(stateMachine.CurrentState is PlayerAttack02 || stateMachine.CurrentState is PlayerAttack03))
        {
            stateMachine.ChangeState(stateMachine.Attack01State);
        }
    }

    protected override void NoContactWithGround(Collider collider)
    {
        if (!IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    private bool IsGrounded()
    {
        //Debug.Log("Se ha ejecutado el método IsGrounded");

        float radius = groundedData.GroundCheckDistance;
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;
        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);
        //Debug.Log($"Número de colisiones detectadas: {colliders.Length}");

        foreach (Collider collider in colliders)
        {
            string objName = collider.gameObject.name;
            string layerName = LayerMask.LayerToName(collider.gameObject.layer);
            bool isTrigger = collider.isTrigger;

            //Debug.Log($"Detectado: {objName} | Capa: {layerName} | isTrigger: {isTrigger}");

            if (collider.gameObject.layer == LayerMask.NameToLayer("Enviroment") && !collider.isTrigger)
            {
                //Debug.Log("Se ha detectado suelo");
                return true;
            }
        }
        return false;
    }
}
