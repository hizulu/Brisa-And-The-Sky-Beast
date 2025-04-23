/*
 * NOMBRE CLASE: PlayerMovedState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/03/2025
 * DESCRIPCI�N: Clase que hereda de PlayerGroundedState.
 *              Estado padre que contiene los subestados de movimiento de Player.
 *              (Este estado es importante por la gesti�n del Animator).
 * VERSI�N: 1.0. 
 */
public class PlayerMovedState : PlayerGroundedState
{
    public PlayerMovedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.MovedParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.MovedParameterHash);
    }
    #endregion
}