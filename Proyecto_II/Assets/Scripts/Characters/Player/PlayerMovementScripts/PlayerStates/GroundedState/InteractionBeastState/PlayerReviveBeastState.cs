
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerReviveBeastState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 25/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerInteractionState y contiene la lógica de revivir a la Bestia.
 * VERSIÓN: 1.0.
 */

public class PlayerReviveBeastState : PlayerInteractionState
{
    public PlayerReviveBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    private float maxTimeToRevive = 3f;
    private float currentTime;

    public override void Enter()
    {
        currentTime = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.ReviveBeastParameterHash);

        // Indicar que estamos en proceso de revivir
        HalfDeadScreen.Instance.IsReviving = true;
        HalfDeadScreen.Instance.ShowHalfDeadScreenBestiaRevive(0f);

        Debug.Log("Has entrado en el estado de REVIVIR A LA BESTIA");
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.canceled += OnReviveCanceled;
    }


    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (currentTime < maxTimeToRevive)
        {
            ReviveBeast();
            //Debug.Log(currentTime);
        }
    }

    public override void Exit()
    {
        // Restablecer el estado de revivir al salir
        HalfDeadScreen.Instance.IsReviving = false;
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.canceled -= OnReviveCanceled;
        currentTime = 0f;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.ReviveBeastParameterHash);
        Debug.Log("Has salido del estado de REVIVIR A LA BESTIA");
    }

    public void ReviveBeast()
    {
        currentTime += Time.deltaTime;
        float normalizedTime = currentTime / maxTimeToRevive;
        HalfDeadScreen.Instance.ShowHalfDeadScreenBestiaRevive(normalizedTime);

        if (currentTime >= maxTimeToRevive)
        {
            EventsManager.TriggerNormalEvent("ReviveBeast");
            Debug.Log("La Bestia ha sido revivida.");
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void OnReviveCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}