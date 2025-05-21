using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerCrouchState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 11/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acci�n de andar en sigilo.
 * VERSI�N: 1.0. 
 */
public class PlayerCrouchState : PlayerMovedState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
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

    #region M�todos Propios CrouchState
    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� en sigilo.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.44f, 0f));
        SetFaceProperty(2, new Vector2(0.875f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion

    #region M�todo Cancelar Entrada Input
    /// <summary>
    /// M�todo sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (!stateMachine.Player.PlayerInput.PlayerActions.Crouch.IsPressed())
            OnStop();
        else
            stateMachine.ChangeState(stateMachine.CrouchPoseState);
    }
    #endregion
}
