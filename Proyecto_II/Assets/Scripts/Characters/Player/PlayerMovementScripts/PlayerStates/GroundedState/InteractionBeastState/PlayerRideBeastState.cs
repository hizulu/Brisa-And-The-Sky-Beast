using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: PlayerRideBeastState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 18/04/2025
 * DESCRIPCI�N: Clase que hereda de PlayerInteractionState.
 *              Subestado que gestiona la acci�n de montar a la Bestia.
 * VERSI�N: 1.0.
 */
public class PlayerRideBeastState : PlayerInteractionState
{
    public PlayerRideBeastState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        canFall = false;
        stateMachine.MovementData.MovementSpeedModifier = groundedData.RideBeastData.RideBeastSpeedModif;
        SetRandomBlink();
        base.Enter();
        stateMachine.Player.PlayerInput.PlayerActions.DesmountBeast.Enable();
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.Disable();
        Debug.Log("Has entrado en el estado de MONTAR A LA BESTIA");
        StartAnimation(stateMachine.Player.PlayerAnimationData.RideBeastParameterHash);
        stateMachine.Player.PlayerInput.PlayerActions.DesmountBeast.performed += DismountBeast;
        EventsManager.CallNormalEvents("EnsureBrisaDismounts", DismountCalledFromBeast);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        //Debug.Log("Est�s en el estado de MONTAR A LA BESTIA");
        base.UpdateLogic();
        HandleBlinking();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        canFall = true;
        base.Exit();
        stateMachine.Player.PlayerInput.PlayerActions.DesmountBeast.Disable();
        stateMachine.Player.PlayerInput.PlayerActions.Crouch.Disable();
        Debug.Log("Has salido del estado de MONTAR A LA BESTIA");
        StopAnimation(stateMachine.Player.PlayerAnimationData.RideBeastParameterHash);
        stateMachine.Player.PlayerInput.PlayerActions.DesmountBeast.performed -= DismountBeast;
        EventsManager.StopCallNormalEvents("EnsureBrisaDismounts", DismountCalledFromBeast);
    }
    #endregion

    #region M�todos Propios RideBeastState
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">Informaci�n del input asociado a la acci�n.</param>
    private void DismountBeast(InputAction.CallbackContext context)
    {
        Debug.Log("Quieres bajarte de la BESTIA");
        stateMachine.ChangeState(stateMachine.DismountBeastState);
    }

    private void DismountCalledFromBeast()
    {
        Debug.Log("BESTIA ha salido del estado de montar");
        stateMachine.ChangeState(stateMachine.DismountBeastState);
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� montada en la Bestia.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.885f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}
