/*
 * NOMBRE CLASE: PlayerMovedState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/03/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundedState.
 *              Estado padre que contiene los subestados de movimiento de Player.
 *              (Este estado es importante por la gestión del Animator).
 * VERSIÓN: 1.0. 
 */
public class PlayerMovedState : PlayerGroundedState
{
    public PlayerMovedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
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