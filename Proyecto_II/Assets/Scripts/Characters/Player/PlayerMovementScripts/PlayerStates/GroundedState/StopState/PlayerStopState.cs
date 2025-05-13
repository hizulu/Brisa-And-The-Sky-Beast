using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerHardLandState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 23/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundedState.
 *              Subestado que gestiona la acción de aterrizar.
 * VERSIÓN: 1.0. 
 */

public class PlayerStopState : PlayerGroundedState
{
    public PlayerStopState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en estado de ATERRIZAR");
        StartAnimation(stateMachine.Player.PlayerAnimationData.StopParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Has salido del estado de ATERRIZAR");
        StopAnimation(stateMachine.Player.PlayerAnimationData.StopParameterHash);
    }
}
