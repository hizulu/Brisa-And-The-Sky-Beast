using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE SCRIPT: PlayerGroundedState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState
 * VERSI�N: 1.0.
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
        Debug.Log("Llamando a NoContactWithGround en PlayerGroundedState");

        if (IsGrounded())
        {
            Debug.Log("Est�s tocando suelo.");
            return;
        }

        stateMachine.ChangeState(stateMachine.FallState);
        Debug.Log("Est�s cayendo");
    }

    private bool IsGrounded()
    {
        // Define el radio de la esfera que detectar� el suelo
        float radius = groundedData.GroundCheckDistance;  // Usamos el valor del radio definido en groundedData
        Debug.Log($"Radio de detecci�n: {radius}");  // Verifica el valor del radio

        // Usamos la posici�n del centro del BoxCollider de GroundCheck
        Vector3 groundCheckPosition = stateMachine.Player.GroundCheckCollider.transform.position;
        Debug.Log($"Posici�n del chequeo de suelo: {groundCheckPosition}");  // Verifica la posici�n del centro del collider

        // Detecta los colliders en un �rea alrededor del centro del BoxCollider
        Collider[] colliders = Physics.OverlapSphere(groundCheckPosition, radius);
        Debug.Log($"Colliders detectados: {colliders.Length}");  // Verifica cu�ntos colliders se detectan

        foreach (Collider collider in colliders)
        {
            // Imprime el nombre del objeto para verificar lo que se est� detectando
            Debug.Log($"Collider detectado: {collider.gameObject.name}, Layer: {LayerMask.LayerToName(collider.gameObject.layer)}");

            // Verifica si el collider est� en la capa de "Ground" y no es trigger
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") && !collider.isTrigger)
            {
                Debug.Log("�El jugador est� tocando el suelo!");
                return true;  // El jugador est� tocando el suelo
            }
        }

        Debug.Log("El jugador no est� tocando el suelo.");
        return false;  // El jugador no est� tocando el suelo
    }
}
