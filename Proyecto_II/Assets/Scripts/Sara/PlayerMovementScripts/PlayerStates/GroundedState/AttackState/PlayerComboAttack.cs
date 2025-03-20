using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComboAttack : PlayerAttackState
{
    public PlayerComboAttack(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    private bool attackFinish;

    public override void Enter()
    {
        attackFinish = false;
        base.Enter();
        StartAnimation(playerStateMachine.Player.PlayerAnimationData.Attack01ParameterHash);
    }

    public override void UpdateLogic()
    {
        FinishAttack01();
    }

    public override void Exit()
    {
        attackFinish = false;
        base.Exit();
        StopAnimation(playerStateMachine.Player.PlayerAnimationData.Attack01ParameterHash);
    }

    private void FinishAttack01()
    {
        Animator animator = playerStateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            attackFinish = true;
            playerStateMachine.ChangeState(playerStateMachine.IdleState);
        }
    }

    protected override void Move()
    {
        if (!attackFinish)
            return;
    }

    // Combo de Ataques
}
