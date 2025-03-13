using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE SCRIPT: PlayerMovementState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de IState
 * VERSIÓN: 1.0.
 */
public class PlayerMovementState : IState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerGroundedData groundedData;

    public PlayerMovementState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        groundedData = stateMachine.Player.Data.GroundedData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void UpdateLogic()
    {
        //Debug.Log("Actualizando");
    }

    public virtual void UpdatePhysics()
    {
        Move();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementData.MovementInput = stateMachine.Player.playerInput.PlayerActions.Movement.ReadValue<Vector2>();
    }

    protected void StartAnimation(int hashNumAnimation)
    {
        stateMachine.Player.animPlayer.SetBool(hashNumAnimation, true);
    }

    protected void StopAnimation(int hashNumAnimation)
    {
        stateMachine.Player.animPlayer.SetBool(hashNumAnimation, false);
    }

    protected virtual void AddInputActionsCallbacks()
    {
        stateMachine.Player.playerInput.PlayerActions.Movement.canceled += OnMovementCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        stateMachine.Player.playerInput.PlayerActions.Movement.canceled -= OnMovementCanceled;
    }

    private void Move()
    {
        if (stateMachine.MovementData.MovementInput == Vector2.zero || stateMachine.MovementData.MovementSpeedModifier == 0f)
            return;

        Vector3 movementDirection = GetMovementInputDirection();
        float movSpeed = GetMovementSpeed();
        movementDirection.Normalize();
        stateMachine.Player.rbPlayer.MovePosition(stateMachine.Player.rbPlayer.position + movementDirection * movSpeed * Time.deltaTime);
    }

    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.MovementData.MovementInput.x, 0f, stateMachine.MovementData.MovementInput.y);
    }

    protected float GetMovementSpeed()
    {
        float movementSpeed = groundedData.BaseSpeed * stateMachine.MovementData.MovementSpeedModifier;

        return movementSpeed;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Este es el script BASE.");
    }
}
