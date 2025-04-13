using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack01 : PlayerAttackState
{
    //public static event Action<float> OnAttack01Enemy;

    public PlayerAttack01(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        maxTimeToNextAttack = 0.5f;
        attackTimeElapsed = 0;
        attackFinish = false;
        attackDamageModifier = 1f;
        base.Enter();
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack01ParameterHash);

        float attackDamageCombo01 = stateMachine.StatsData.AttackDamageBase * attackDamageModifier;
        EventsManager.TriggerSpecialEvent<float>("OnAttack01Enemy", attackDamageCombo01);
        //OnAttack01Enemy?.Invoke(attackDamageCombo01);
        //Debug.Log("Daño del ataque 1: " + " " + attackDamageCombo01);
    }

    public override void HandleInput()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Attack.triggered && !isWaitingForInput)
        {
            canContinueCombo = true;
            isWaitingForInput = true;
        }

        if (attackFinish && canContinueCombo)
        {
            if (attackTimeElapsed < maxTimeToNextAttack && isWaitingForInput)
            {
                stateMachine.ChangeState(stateMachine.Attack02State);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    public override void UpdateLogic()
    {
        FinishAttack();
        attackTimeElapsed += Time.deltaTime;
    }

    public override void Exit()
    {
        canContinueCombo = false;
        isWaitingForInput = false;
        attackFinish = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack01ParameterHash);
    }

    protected override void FinishAttack()
    {
        //Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            attackFinish = true;
        }
    }

    protected override void Move()
    {
        if (!attackFinish)
            return;
    }
}
