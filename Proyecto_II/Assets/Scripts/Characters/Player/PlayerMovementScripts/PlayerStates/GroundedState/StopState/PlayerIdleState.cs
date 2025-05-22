using UnityEngine;

/*
 * NOMBRE CLASE: PlayerIdleState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerGroundeState
 *              Subestado que gestiona la acci�n de estar parado.
 * VERSI�N: 1.0. 
 */
public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementData.MovementSpeedModifier = 0f;
        SetRandomBlink();
        StartAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        Debug.Log("Has entrado en el estado de IDLE.");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (stateMachine.MovementData.MovementInput == Vector2.zero)
        {
            HandleBlinking();
            return;
        }

        OnMove();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.IdleParameterHash);
        Debug.Log("Has salido del estado de IDLE.");
    }
    #endregion

    #region M�todos Propios IdleState
    /// <summary>
    /// M�todo para cambiar la expresi�n de Brisa cuando est� en en idle.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0f, 0f));
        SetFaceProperty(2, new Vector2(0f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}
