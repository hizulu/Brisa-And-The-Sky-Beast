using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealBeastState : PlayerInteractionState
{
    public PlayerHealBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Variables
    private bool healBeastFinish;
    #endregion

    #region M�todos Base de la M�quina de Estados
    public override void Enter()
    {
        healBeastFinish = false;
        stateMachine.Player.Mango.SetActive(true);
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.HealBeastParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.HealBeastParameterHash);
    }
    #endregion

    #region M�todos Propios HealBeastState
    /*
     * M�todo para comprobar que la animaci�n de acariciar se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("HealBeast") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            stateMachine.Player.Mango.SetActive(false);
            healBeastFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    #endregion
}
