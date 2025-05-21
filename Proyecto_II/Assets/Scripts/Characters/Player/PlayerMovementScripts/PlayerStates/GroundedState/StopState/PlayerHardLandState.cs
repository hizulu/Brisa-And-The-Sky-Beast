using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: PlayerHardLandState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 23/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerStopState.
 *              Subestado que gestiona la acción de aterrizar.
 * VERSIÓN: 1.0. 
 */

public class PlayerHardLandState : PlayerStopState
{
    public PlayerHardLandState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    private float fallDamage = 15f;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Disable();
        statsData.CurrentHealth -= fallDamage;
        base.Enter();
        //Debug.Log("Has entrado en estado de ATERRIZAR");
        StartAnimation(stateMachine.Player.PlayerAnimationData.HardLandParameterHash);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.HardLand);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishLand();

        if (statsData.CurrentHealth < Mathf.Epsilon)
            PlayerDead();
    }

    public override void Exit()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Enable();
        base.Exit();
        //Debug.Log("Has salido del estado de ATERRIZAR");
        StopAnimation(stateMachine.Player.PlayerAnimationData.HardLandParameterHash);
    }
    #endregion

    #region Métodos Propios HardLandState
    /*
     * Método para comprobar que la animación de aterrizar se ha terminado para pasar al siguiente estado requerido.
     */
    private void FinishLand()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("HardLand") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    /*
     * Método para cambiar la expresión de Brisa al aterrizar desde muy alto.
     */
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(1.22f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0.655f, 0f));
    }
    #endregion
}
