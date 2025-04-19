/*
 * NOMBRE CLASE: PlayerDeathState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState
 * VERSI�N: 1.0. 
 */
public class PlayerDeathState : PlayerMovementState
{
    public PlayerDeathState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
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
        base.Exit();
        //Debug.Log("Has salido del estado de MUERTE");
        StopAnimation(stateMachine.Player.PlayerAnimationData.DeathParameterHash);
    }
    #endregion
}
