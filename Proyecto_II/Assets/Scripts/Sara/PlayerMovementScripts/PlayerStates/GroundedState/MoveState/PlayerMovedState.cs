/*
 * NOMBRE CLASE: PlayerMovedState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que hereda de PlayerGroundedState
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