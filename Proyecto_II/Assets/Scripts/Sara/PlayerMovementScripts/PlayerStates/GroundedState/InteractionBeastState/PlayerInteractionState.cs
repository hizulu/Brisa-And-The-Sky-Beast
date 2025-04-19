/*
 * NOMBRE CLASE: PlayerInteractionState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que hereda de PlayerGroundedState
 * VERSI�N: 1.0. 
 */
public class PlayerInteractionState : PlayerGroundedState
{
    public PlayerInteractionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.InteractionsParameterHash);
        //Debug.Log("Has entrado en estado de Interacci�n con la Bestia.");
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
        //Debug.Log("Has salido del estado de Interacci�n con la Bestia.");
    }
    #endregion
}
