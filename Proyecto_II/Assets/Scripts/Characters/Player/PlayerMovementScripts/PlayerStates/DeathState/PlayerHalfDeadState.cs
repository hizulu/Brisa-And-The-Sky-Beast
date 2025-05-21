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

    #region Variables
    private Beast beast;
    private BeastTrapped beastTrapped;
    private bool halfDeadAnimPlayer = false;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        EventsManager.TriggerNormalEvent("BrisaHalfDead");
        EventsManager.CallNormalEvents("BrisaRevive", PlayerRevive);
        halfDeadAnimPlayer = false;
        base.Enter();
        Debug.Log("Has entrado en el estado de MEDIO-MUERTA");
        //statsData.CurrentTimeHalfDead = 60f;
        statsData.CurrentTimeHalfDead = statsData.MaxTimeHalfDead;
        StartAnimation(stateMachine.Player.PlayerAnimationData.HalfDeadParameterHash);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.HalfDeath);
        beast = stateMachine.Player.Beast;
        beastTrapped = stateMachine.Player.beastTrapped;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();

        if (halfDeadAnimPlayer)
            IdleHalfDeadAnimation();

        TimeToRevivePlayer();
    }

    public override void Exit()
    {
        EventsManager.StopCallNormalEvents("BrisaRevive", PlayerRevive);
        //isHalfDead = false;
        base.Exit();
        Debug.Log("Has salido del estado de MEDIO-MUERTA");
        StopAnimation(stateMachine.Player.PlayerAnimationData.IdleHalfDeadParameterHash);
        //StopAnimation(stateMachine.Player.PlayerAnimationData.HalfDeadParameterHash);
    }
    #endregion

    #region M�todos Propios HalfDeadState
    /// <summary>
    /// M�todo que realiza la cuenta atr�s para que la Bestia pueda revivir a Player.
    /// Si el tiempo se acaba, muere definitivamente.
    /// </summary>
    private void TimeToRevivePlayer()
    {
        // Debug.Log("Est�s medio - muerta");
        statsData.CurrentTimeHalfDead -= Time.deltaTime;
        if(beastTrapped.beasIsFree)
            HalfDeadScreen.Instance.ShowHalfDeadScreenBrisa(statsData.CurrentTimeHalfDead, statsData.MaxTimeHalfDead);

        if (statsData.CurrentTimeHalfDead <= 0 || !beastTrapped.beasIsFree)
            stateMachine.ChangeState(stateMachine.FinalDeadState);
    }

    /// <summary>
    /// M�todo que revive a Brisa si Bestia ha conseguido cumplir sus condiciones.
    /// Si se consigue, cambia el estado a ReviveState.
    /// </summary>
    private void PlayerRevive()
    {
        beast.SetBrisaHalfDead(false);
        HalfDeadScreen.Instance.HideHalfDeadScreenBrisa();
        stateMachine.ChangeState(stateMachine.RevivePlayerState);
    }

    /// <summary>
    /// M�todo sobreescrito que gestiona que, al terminar la primera parte de la animaci�n de medio-muerta, de paso a la animaci�n de idle medio-muerta.
    /// </summary>
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("HalfDead") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            StopAnimation(stateMachine.Player.PlayerAnimationData.HalfDeadParameterHash);
            halfDeadAnimPlayer = true;
        }
    }

    /// <summary>
    /// Empieza la animaci�n de idle medio-muerta.
    /// </summary>
    private void IdleHalfDeadAnimation()
    {
        StartAnimation(stateMachine.Player.PlayerAnimationData.IdleHalfDeadParameterHash);
    }

    /// <summary>
    /// M�todo sobreescrito para cambiar la expresi�n de Brisa cuando est� esperando a que Bestia la reviva.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.44f, 0f));
        SetFaceProperty(2, new Vector2(0.5f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
