using UnityEngine;

/*
 * NOMBRE CLASE: PlayerTakeDamageState
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 12/04/2025
 * DESCRIPCIÓN: Estado de recibir daño.
 * VERSIÓN: 1.0. 
 */
public class PlayerTakeDamageState : PlayerMovementState
{
    public PlayerTakeDamageState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.TakeDamageParameterHash);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.TakeDamage);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishTakeDamage();
    }

    public override void Exit()
    {
        //stateMachine.Player.SfxPlayer.StopSound(BrisaSFXType.TakeDamage);
        base.Exit(); 
        StopAnimation(stateMachine.Player.PlayerAnimationData.TakeDamageParameterHash);
    }
    #endregion

    #region Métodos Propios TakeDamageState
    /// <summary>
    /// Método sobreescrito que gestiona que al terminar la animación de recibir daño, pasa a IdleState.
    /// </summary>
    private void FinishTakeDamage()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    /// <summary>
    /// Método sobreescrito para cambiar la expresión de Brisa cuando está recibiendo daño.
    /// </summary>
    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.22f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0.33f, 0f));
    }
    #endregion
}
