using UnityEngine;

/*
 * NOMBRE CLASE: PlayerHalfDeadState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Clase que hereda de PlayerDeathState.
 *              Subestado donde Player está en estado de "medio-muerta".
 *              No puede realizar ninguna acción.
 *              La Bestia tiene un tiempo limitado para revivir a Player, sino, pasa a muerte definitiva.
 * VERSIÓN: 1.0. 
 */
public class PlayerHalfDeadState : PlayerDeathState
{
    public PlayerHalfDeadState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
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

    #region Métodos Propios HalfDeadState
    /*
     * Método que realiza la cuenta atrás para que la Bestia pueda revivir a Player.
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
