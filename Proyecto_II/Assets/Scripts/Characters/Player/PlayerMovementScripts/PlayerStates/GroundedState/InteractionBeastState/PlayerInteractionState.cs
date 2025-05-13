/*
 * NOMBRE CLASE: PlayerInteractionState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 14/04/2025
 * DESCRIPCIÓN: Estado padre del que heredan los subestados de las diferentes interacciones con la Bestia.
 * VERSIÓN: 1.0. 
 */
public class PlayerInteractionState : PlayerGroundedState
{
    public PlayerInteractionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.InteractionsParameterHash);
        //Debug.Log("Has entrado en estado de Interacción con la Bestia.");
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
        StopAnimation(stateMachine.Player.PlayerAnimationData.InteractionsParameterHash);
        //Debug.Log("Has salido del estado de Interacción con la Bestia.");
    }
    #endregion
}
