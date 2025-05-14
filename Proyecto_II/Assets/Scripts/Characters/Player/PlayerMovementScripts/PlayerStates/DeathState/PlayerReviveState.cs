using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerReviveState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 06/05/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerDeathState.
 *              Subestado que gestiona la lógica de cuando Player es revivida por la Bestia.
 * VERSIÓN: 1.0. 
 */

public class PlayerReviveState : PlayerDeathState
{
    public PlayerReviveState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Has entrado en el estado de REVIVIR");
        StartAnimation(stateMachine.Player.PlayerAnimationData.RevivePlayerParameterHash);

        statsData.CurrentHealth = statsData.MaxHealth / 2;
        EventsManager.TriggerSpecialEvent<float>("PlayerHealth", statsData.CurrentHealth);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Has salido del estado de REVIVIR");
        StopAnimation(stateMachine.Player.PlayerAnimationData.RevivePlayerParameterHash);
    }
    #endregion

    #region Métodos Propios ReviveState
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Revive") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.885f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
