using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerCrouchState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 11/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acción de andar en sigilo.
 * VERSIÓN: 1.0. 
 */
public class PlayerCrouchState : PlayerMovedState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        stateMachine.MovementData.MovementSpeedModifier = groundedData.CrouchData.CrouchSpeedModif;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        EventsManager.TriggerSpecialEvent<bool>("PlayerCrouchState", true);
        //Debug.Log("Has entrado en el estado de AGACHARSE.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.Crouch);
    }

    public override void Exit()
    {
        stateMachine.Player.SfxPlayer.StopSound(BrisaSFXType.Crouch);
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.CrouchParameterHash);
        EventsManager.TriggerSpecialEvent<bool>("PlayerCrouchState", false);
        //Debug.Log("Has salido del estado de AGACHARSE.");
    }
    #endregion

    #region Métodos Propios CrouchState
    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está en sigilo.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.44f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion

    #region Método Cancelar Entrada Input
    /// <summary>
    /// Método sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
    /// </summary>
    /// <param name="context">Información del input asociado a la acción.</param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (!stateMachine.Player.PlayerInput.PlayerActions.Crouch.IsPressed())
            OnStop();
        else
            stateMachine.ChangeState(stateMachine.CrouchPoseState);
    }
    #endregion
}
