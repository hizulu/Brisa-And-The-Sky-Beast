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

    #region Métodos Propios RunState
    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está corriendo.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.78f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion

    #region Método Cancelar Entrada Input
    /// <summary>
    /// Método sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        OnStop();
    }
    #endregion
}
