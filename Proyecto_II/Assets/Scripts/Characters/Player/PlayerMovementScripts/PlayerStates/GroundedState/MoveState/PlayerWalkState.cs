using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerWalkState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2015
 * DESCRIPCI�N: Clase que hereda de PlayerMovedState.
 *              Subestado que gestiona la acci�n de caminar.
 * VERSI�N: 1.0. 
 */
public class PlayerWalkState : PlayerMovedState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
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

    #region M�todos Propios WalkState
    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� caminando.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.555f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion

    #region M�todo Cancelar Entrada Input
    /// <summary>
    /// M�todo sobrescrito que se ejecuta cuando se cancela la entrada de movimiento.
    /// Si no detecta ning�n input, pasa al estado de idle.
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;

        OnStop();
    }
    #endregion
}
