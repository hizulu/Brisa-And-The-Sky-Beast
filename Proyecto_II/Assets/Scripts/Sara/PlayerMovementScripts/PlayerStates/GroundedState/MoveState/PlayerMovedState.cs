/*
 * NOMBRE CLASE: PlayerMovedState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que hereda de PlayerGroundedState
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