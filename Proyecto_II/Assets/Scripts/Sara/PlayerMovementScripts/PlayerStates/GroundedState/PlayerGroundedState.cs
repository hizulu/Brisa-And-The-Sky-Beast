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
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.GroundedParameterHash);
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
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.GroundedParameterHash);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        playerStateMachine.Player.PlayerInput.PlayerActions.Jump.started += JumpStarted;
        playerStateMachine.Player.PlayerInput.PlayerActions.Crouch.started += CrouchStarted;
        playerStateMachine.Player.PlayerInput.PlayerActions.Attack.started += AttackStart;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        playerStateMachine.Player.PlayerInput.PlayerActions.Jump.started -= JumpStarted;
        playerStateMachine.Player.PlayerInput.PlayerActions.Crouch.started -= CrouchStarted;
        playerStateMachine.Player.PlayerInput.PlayerActions.Attack.started -= AttackStart;
    }

    protected virtual void OnMove()
    {
        playerStateMachine.ChangeState(playerStateMachine.WalkState);
    }

    protected virtual void OnStop()
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
    }

    protected virtual void JumpStarted(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.JumpState);
    }

    protected virtual void CrouchStarted(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.CrouchState);
    }

    protected virtual void AttackStart(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.ComboAttack);
    }

    protected override void NoContactWithGround(Collider collider)
    {
        Debug.Log("Llamando a NoContactWithGround en PlayerGroundedState");

        if (IsGrounded())
        {
            Debug.Log("Est�s tocando suelo.");
            return;
        }

        playerStateMachine.ChangeState(playerStateMachine.FallState);
        Debug.Log("Est�s cayendo");
    }

    private bool IsGrounded()
    {
        // Define el radio de la esfera que detectar� el suelo
        float radius = groundedData.GroundCheckDistance;  // Usamos el valor del radio definido en groundedData
        Debug.Log($"Radio de detecci�n: {radius}");  // Verifica el valor del radio

        // Usamos la posici�n del centro del BoxCollider de GroundCheck
        Vector3 groundCheckPosition = playerStateMachine.Player.GroundCheckCollider.transform.position;
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
