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
        Debug.Log("Has entrado en el estado de REVIVIR A LA BESTIA");
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.canceled += OnReviveCanceled;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (currentTime < maxTimeToRevive)
        {
            ReviveBeast();
            Debug.Log(currentTime);
        }
        else
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public override void Exit()
    {
        stateMachine.Player.PlayerInput.PlayerActions.ReviveBeast.canceled -= OnReviveCanceled;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.ReviveBeastParameterHash);
        Debug.Log("Has salido del estado de REVIVIR A LA BESTIA");
    }

    public void ReviveBeast()
    {
        Debug.Log("Brisa está reviviendo a Bestia");
        float healPerSecond = stateMachine.Player.Beast.maxHealth / maxTimeToRevive;
        stateMachine.Player.Beast.currentHealth += healPerSecond * Time.deltaTime;

        if (stateMachine.Player.Beast.currentHealth > stateMachine.Player.Beast.maxHealth)
        {
            stateMachine.Player.Beast.currentHealth = stateMachine.Player.Beast.maxHealth;
            Debug.Log("La Bestia ha sido revivida.");
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        currentTime += Time.deltaTime;
        HalfDeadScreen.Instance.ShowHalfDeadScreenBestiaRevive(currentTime);
    }

    protected override void OnReviveCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}