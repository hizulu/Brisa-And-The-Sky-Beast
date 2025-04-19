/*
 * NOMBRE CLASE: PlayerLandState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 
 * DESCRIPCI�N: Clase que hereda de PlayerGroundedState
 * VERSI�N: 1.0. 
 */
public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    private bool landFinish;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        landFinish = false;
        base.Enter();
        //Debug.Log("Has entrado en estado de ATERRIZAR");
        StartAnimation(stateMachine.Player.PlayerAnimationData.LandParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishLand();
    }

    public override void Exit()
    {
        landFinish = false;
        base.Exit();
        //Debug.Log("Has salido del estado de ATERRIZAR");
        StopAnimation(stateMachine.Player.PlayerAnimationData.LandParameterHash);
    }
    #endregion

    #region M�todo Propio LandState
    /*
     * M�todo para comprobar que la animaci�n de aterrizar se ha terminado para pasar al siguiente estado requerido.
     */
    private void FinishLand()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("Aterrizaje") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            landFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    #endregion
}
