using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallBeastState : PlayerInteractionState
{
    public PlayerCallBeastState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    #region Métodos Base de la Máquina de Estados
    public override void Enter()
    {
        EventsManager.TriggerNormalEvent("CallBeast");
        base.Enter();
        Debug.Log("Has entrado en el estado de LLAMAR A LA BESTIA");
        StartAnimation(stateMachine.Player.PlayerAnimationData.CallBeastParameterHash);
        stateMachine.Player.SfxPlayer.PlayRandomSFX(BrisaSFXType.CallBeast, 0.6f);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishAnimation();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Has salido del estado de LLAMAR A LA BESTIA");
        StopAnimation(stateMachine.Player.PlayerAnimationData.CallBeastParameterHash);
    }
    #endregion

    #region Métodos Propios PetBeastState
    /*
     * Método para comprobar que la animación de acariciar se ha terminado para pasar al siguiente estado requerido.
     */
    protected override void FinishAnimation()
    {
        if (stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).IsName("CallBeast") && stateMachine.Player.AnimPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected override void ChangeFacePlayer()
    {
        base.ChangeFacePlayer();

        SetFaceProperty(1, new Vector2(0.33f, 0f));
        SetFaceProperty(2, new Vector2(0.375f, 0f));
        SetFaceProperty(3, new Vector2(0f, 0f));
    }
    #endregion
}
