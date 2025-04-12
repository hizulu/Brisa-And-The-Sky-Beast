using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPetBeastState : PlayerInteractionState
{
    public PlayerPetBeastState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    private bool petBeastFinish;

    public override void Enter()
    {
        petBeastFinish = false;
        base.Enter();
        Debug.Log("Has entrado en estado de Acariciar a la Bestia.");
        StartAnimation(stateMachine.Player.PlayerAnimationData.PetBeastParameterHash);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        FinishPettingBeast();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Has salido del estado de Acariciar a la Bestia.");
        StopAnimation(stateMachine.Player.PlayerAnimationData.PetBeastParameterHash);
    }

    private void FinishPettingBeast()
    {
        Animator animator = stateMachine.Player.AnimPlayer;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AcariciarBestia_Brisa") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            petBeastFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
