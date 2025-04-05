using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack03 : PlayerAttackState
{
    public PlayerAttack03(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        attackFinish = false;
        base.Enter();
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack03ParameterHash);
    }

    public override void UpdateLogic()
    {
        FinishAttack();
    }

    public override void Exit()
    {
        attackFinish = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack03ParameterHash);
    }

    protected override void FinishAttack()
    {
        //Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            attackFinish = true;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void Move()
    {
        if (!attackFinish)
            return;
    }
}
