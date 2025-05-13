/*
 * NOMBRE CLASE: PlayerDeathState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerMovementState.
 *              Estado padre que contiene los subestados de muerte del Player.
 * VERSIÓN: 1.0. 
 */
public class PlayerDeathState : PlayerMovementState
{
    public PlayerDeathState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
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
