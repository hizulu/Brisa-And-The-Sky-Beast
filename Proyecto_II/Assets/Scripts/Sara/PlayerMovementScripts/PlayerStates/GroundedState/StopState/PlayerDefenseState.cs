using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerDefenseState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 25/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovementState y contiene la lógica de defensa con un escudo.
 * VERSIÓN: 1.0.
 */

public class PlayerDefenseState : PlayerMovementState
{
    public PlayerDefenseState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private float maxTimeWithShield = 5f;
    private float currentTime;

    public override void Enter()
    {
        currentTime = 0f;
        base.Enter();
        //StartAnimation(stateMachine.Player.PlayerAnimationData.DefenseParameterHash);
        Debug.Log("Has entrado en el estado de DEFENSA");
        stateMachine.Player.PlayerInput.PlayerActions.Shield.canceled += OnDefendedCanceled;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        TimeWithShield();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.PlayerAnimationData.DefenseParameterHash);
        Debug.Log("Has salido del estado de DEFENSA");
    }

    private void TimeWithShield()
    {
        if (currentTime < maxTimeWithShield)
        {
            currentTime += Time.deltaTime;
            Debug.Log("Te estás defendiendo con un escudo.");
        }
        else
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected override void OnDefendedCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
