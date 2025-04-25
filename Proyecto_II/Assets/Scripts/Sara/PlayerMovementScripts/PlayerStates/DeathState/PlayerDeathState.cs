/*
 * NOMBRE CLASE: PlayerDeathState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 03/04/2025
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState.
 *              Estado padre que contiene los subestados de muerte del Player.
 * VERSI�N: 1.0. 
 */
public class PlayerDeathState : PlayerMovementState
{
    public PlayerDeathState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Disable();
        base.Enter();
        //Debug.Log("Has entrado en el estado de MUERTE");
        StartAnimation(stateMachine.Player.PlayerAnimationData.DeathParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        stateMachine.Player.PlayerInput.PlayerActions.Enable();
        base.Exit();
        //Debug.Log("Has salido del estado de MUERTE");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DeathParameterHash);
    }
    #endregion
}
