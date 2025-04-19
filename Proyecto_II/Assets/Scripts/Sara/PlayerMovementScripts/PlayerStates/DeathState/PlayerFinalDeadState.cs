/*
 * NOMBRE CLASE: PlayerFinalDeadState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que hereda de PlayerDeathState
 * VERSIÓN: 1.0. 
 */
public class PlayerFinalDeadState : PlayerDeathState
{
    public PlayerFinalDeadState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en el estado de MUERTE FINAL");
        StartAnimation(stateMachine.Player.PlayerAnimationData.FinalDeadParameterHash);
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
        //Debug.Log("Has salido del estado de MUERTE FINAL");
        StopAnimation(stateMachine.Player.PlayerAnimationData.FinalDeadParameterHash);
    }
    #endregion
}
