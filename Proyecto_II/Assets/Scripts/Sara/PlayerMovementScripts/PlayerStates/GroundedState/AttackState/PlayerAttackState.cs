using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerGroundedState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        //comboAttacks = 0;
        //totalComboTime = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.PlayerAnimationData.AttackParameterHash);
        Debug.Log("Has entrado en el estado de ATACAR");
    }

    //public override void UpdateLogic()
    //{
    //    if (totalComboTime > btAttacksTime)
    //        ResetCombo();
    //}

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.AttackParameterHash);
        Debug.Log("Has salido del estado de ATACAR");
    }

    //private void ResetCombo()
    //{
    //    comboAttacks = 0;
    //    totalComboTime = 0f;
    //    attackFinish = false;
    //}
}
