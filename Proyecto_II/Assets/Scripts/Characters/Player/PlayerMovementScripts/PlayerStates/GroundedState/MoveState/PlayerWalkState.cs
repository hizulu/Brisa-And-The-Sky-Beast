using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerWalkState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2015
 * DESCRIPCIÓN: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acción de caminar.
 * VERSIÓN: 1.0. 
 */
public class PlayerWalkState : PlayerMovedState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.WalkData.WalkSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.WalkParameterHash);
        Debug.Log("Has entrado en el estado de CAMINAR.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.Walk);
        //audioManager.PlaySFX(audioManager.walk);
    }

    public override void Exit()
    {
        base.Exit();
        //audioManager.StopSFX();
        stateMachine.Player.SfxPlayer.StopSound(BrisaSFXType.Walk);
        StopAnimation(stateMachine.Player.PlayerAnimationData.WalkParameterHash);
        Debug.Log("Has salido del estado de CAMINAR.");
    }
    #endregion

    #region Métodos Propios WalkState
    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está caminando.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.555f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion

    #region Método Cancelar Entrada Input
    /// <summary>
    /// Método sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
    /// Si no detecta ningún input, pasa al estado de idle.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;

        OnStop();
    }
    #endregion
}
