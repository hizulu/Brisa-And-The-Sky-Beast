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
    protected PlayerStateMachine playerStateMachine;

    protected readonly PlayerGroundedData groundedData;
    protected readonly PlayerAirborneData airborneData;

    public PlayerMovementState(PlayerStateMachine _playerStateMachine)
    {
        playerStateMachine = _playerStateMachine;

        groundedData = playerStateMachine.Player.Data.GroundedData;
        airborneData = playerStateMachine.Player.Data.AirborneData;
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

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (playerStateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            ContactWithGround(collider);
            //Debug.Log(collider.gameObject.name);
            return;
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (playerStateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            NoContactWithGround(collider);
            return;
        }
    }

    private void ReadMovementInput()
    {
        playerStateMachine.MovementData.MovementInput = playerStateMachine.Player.PlayerInput.PlayerActions.Movement.ReadValue<Vector2>();
    }

    protected void StartAnimation(int hashNumAnimation)
    {
        playerStateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, true);
    }

    protected void StopAnimation(int hashNumAnimation)
    {
        playerStateMachine.Player.AnimPlayer.SetBool(hashNumAnimation, false);
    }

    protected virtual void AddInputActionsCallbacks()
    {
        playerStateMachine.Player.PlayerInput.PlayerActions.Movement.canceled += OnMovementCanceled;
        playerStateMachine.Player.PlayerInput.PlayerActions.Run.performed += RunStarted;
        playerStateMachine.Player.PlayerInput.PlayerActions.Run.canceled += OnMovementCanceled;
        
        
        
        playerStateMachine.Player.PlayerInput.PlayerActions.Crouch.canceled -= OnMovementCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        playerStateMachine.Player.PlayerInput.PlayerActions.Movement.canceled -= OnMovementCanceled;
        playerStateMachine.Player.PlayerInput.PlayerActions.Run.canceled -= OnMovementCanceled;
    }

    protected virtual void Move()
    {
        if (playerStateMachine.MovementData.MovementInput == Vector2.zero || playerStateMachine.MovementData.MovementSpeedModifier == 0f)
            return;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 movementDirection = cameraForward * playerStateMachine.MovementData.MovementInput.y + Camera.main.transform.right * playerStateMachine.MovementData.MovementInput.x;
        movementDirection.Normalize();

        float movSpeed = GetMovementSpeed();
        movementDirection.Normalize();
        playerStateMachine.Player.RbPlayer.MovePosition(playerStateMachine.Player.RbPlayer.position + movementDirection * movSpeed * Time.deltaTime);
        Rotate(movementDirection);
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection); // Hace que el personaje gire en la dirección donde se produce el movimiento.
            playerStateMachine.Player.RbPlayer.rotation = Quaternion.Slerp(playerStateMachine.Player.RbPlayer.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(playerStateMachine.MovementData.MovementInput.x, 0f, playerStateMachine.MovementData.MovementInput.y);
    }

    protected float GetMovementSpeed()
    {
        float movementSpeed = groundedData.BaseSpeed * playerStateMachine.MovementData.MovementSpeedModifier;

        return movementSpeed;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void RunStarted(InputAction.CallbackContext context)
    {
        playerStateMachine.ChangeState(playerStateMachine.RunState);
    }

    

    

    //private bool IsGrounded()
    //{
    //    return Physics.Raycast(stateMachine.Player.RbPlayer.position, Vector3.down, groundedData.GroundCheckDistance);
    //}

    protected virtual void ContactWithGround(Collider collider)
    {

    }

    protected virtual void NoContactWithGround(Collider collider)
    {

    }

    //public virtual void OnTriggerEnter(Collider collider)
    //{
    //    if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
    //    {
    //        OnContactWithGround(collider);

    //        return;
    //    }
    //}

    //public virtual void OnTriggerExit(Collider collider)
    //{
    //    if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
    //    {
    //        OnContactWithGroundExited(collider);

    //        return;
    //    }
    //}

    //protected virtual void OnContactWithGround(Collider collider)
    //{
    //}

    //protected virtual void OnContactWithGroundExited(Collider collider)
    //{
    //}
}
