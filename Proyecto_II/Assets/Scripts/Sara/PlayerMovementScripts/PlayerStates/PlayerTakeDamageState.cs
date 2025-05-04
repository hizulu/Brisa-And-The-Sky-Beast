using UnityEngine;

/*
 * NOMBRE CLASE: PlayerTakeDamageState
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 12/04/2025
 * DESCRIPCI�N: Estado de recibir da�o.
 * VERSI�N: 1.0. 
 */
public class PlayerTakeDamageState : PlayerMovementState
{
    public PlayerTakeDamageState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Variables
    private bool takeDamageFinish;
    #endregion

    #region M�todos Base de la M�quina de Estados
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

    #region M�todos Propios TakeDamageState
    private void FinishTakeDamage()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            takeDamageFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.22f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
