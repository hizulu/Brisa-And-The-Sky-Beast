using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerRunState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 10/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acción de correr.
 * VERSIÓN: 1.0. 
 */

public class PlayerRunState : PlayerMovedState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.RunData.RunSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.RunParameterHash);
        //Debug.Log("Has entrado en el estado de CORRER.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //AudioManager.Instance.PlaySFX(AudioManager.Instance.run);
        audioManager.PlaySFX(AudioManager.Instance.run);
        // Brisa no puede correr si no está en movimiento.
        if (stateMachine.MovementData.MovementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.Instance.StopSFX();
        audioManager.StopSFX();
        StopAnimation(stateMachine.Player.PlayerAnimationData.RunParameterHash);
        //Debug.Log("Has salido del estado de CORRER.");
    }
    #endregion

    #region Método Cancelar Entrada Input
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMovementCanceled(context);
    }
    #endregion
}
