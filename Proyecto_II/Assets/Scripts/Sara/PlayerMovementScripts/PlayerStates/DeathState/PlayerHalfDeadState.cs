using UnityEngine;

/*
 * NOMBRE CLASE: PlayerHalfDeadState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 03/04/2025
 * DESCRIPCI�N: Clase que hereda de PlayerDeathState.
 *              Subestado donde Player est� en estado de "medio-muerta".
 *              No puede realizar ninguna acci�n.
 *              La Bestia tiene un tiempo limitado para revivir a Player, sino, pasa a muerte definitiva.
 * VERSI�N: 1.0. 
 */
public class PlayerHalfDeadState : PlayerDeathState
{
    public PlayerHalfDeadState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Has entrado en el estado de MEDIO-MUERTA");
        statsData.CurrentTimeHalfDead = 60f;
        statsData.CurrentTimeHalfDead = statsData.MaxTimeHalfDead;
        StartAnimation(stateMachine.Player.PlayerAnimationData.HalfDeadParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        TimeToRevivePlayer();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Has salido del estado de MEDIO-MUERTA");
        StopAnimation(stateMachine.Player.PlayerAnimationData.HalfDeadParameterHash);
    }
    #endregion

    #region M�todos Propios HalfDeadState
    /*
     * M�todo que realiza la cuenta atr�s para que la Bestia pueda revivir a Player.
     * Si el tiempo se acaba, muere definitivamente.
     */
    private void TimeToRevivePlayer()
    {
        statsData.CurrentTimeHalfDead -= Time.deltaTime;

        if (statsData.CurrentTimeHalfDead <= 0)
            stateMachine.ChangeState(stateMachine.FinalDeadState);
    }
    #endregion
}
