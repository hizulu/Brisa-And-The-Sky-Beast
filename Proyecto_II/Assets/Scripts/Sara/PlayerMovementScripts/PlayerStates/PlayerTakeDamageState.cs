/*
 * NOMBRE CLASE: PlayerTakeDamageState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 
 * DESCRIPCIÓN: Clase que hereda de PlayerMovementState
 * VERSIÓN: 1.0. 
 */
public class PlayerTakeDamageState : PlayerMovementState
{
    public PlayerTakeDamageState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private bool takeDamageFinish;
    #endregion

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        takeDamageFinish = false;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.TakeDamageParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishTakeDamage();
    }

    public override void Exit()
    { 
        base.Exit(); 
        StopAnimation(stateMachine.Player.PlayerAnimationData.TakeDamageParameterHash);
    }
    #endregion

    #region Métodos Propios TakeDamageState
    private void FinishTakeDamage()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            takeDamageFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    #endregion
}
