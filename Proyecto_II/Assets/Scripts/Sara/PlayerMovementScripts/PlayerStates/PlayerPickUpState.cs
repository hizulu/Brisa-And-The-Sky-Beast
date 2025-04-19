/*
 * NOMBRE CLASE: PlayerPickUpState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que hereda de PlayerMovementState
 * VERSI�N: 1.0. 
 */
public class PlayerPickUpState : PlayerMovementState
{
    public PlayerPickUpState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private bool pickUpFinish;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        pickUpFinish = false;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.PickUpParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishPickUp();
    }

    public override void Exit() 
    { 
        base.Exit(); 
        StopAnimation(stateMachine.Player.PlayerAnimationData.PickUpParameterHash);
    }
    #endregion

    #region M�todos Propios PickUpState
    private void FinishPickUp()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("PickUp_Brisa") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            pickUpFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    #endregion
}
