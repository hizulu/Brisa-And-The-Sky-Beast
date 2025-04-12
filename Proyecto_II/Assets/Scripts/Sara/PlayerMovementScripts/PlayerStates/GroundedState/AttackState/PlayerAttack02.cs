using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack02 : PlayerAttackState
{
    public PlayerAttack02(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    //public static event Action<float> OnAttack02Enemy;

    public override void Enter()
    {
        maxTimeToNextAttack = 0.7f;
        attackTimeElapsed = 0;
        attackFinish = false;
        attackDamageModifier = 1.5f;
        base.Enter();
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack02ParameterHash);

        float attackDamageCombo02 = stateMachine.StatsData.AttackDamageBase * attackDamageModifier;
        EventsManager.TriggerSpecialEvent<float>("OnAttack02Enemy", attackDamageCombo02);
        //OnAttack02Enemy?.Invoke(attackDamageCombo02);
        Debug.Log("Daño del ataque 2: " + " " + attackDamageCombo02);
    }

    public override void HandleInput()
    {
        if (stateMachine.Player.PlayerInput.PlayerActions.Attack.triggered && attackTimeElapsed < maxTimeToNextAttack)
        {
            canContinueCombo = true;
        }
    }

    public override void UpdateLogic()
    {
        FinishAttack();
        attackTimeElapsed += Time.deltaTime;

        if (attackFinish && canContinueCombo)
        {
            stateMachine.ChangeState(stateMachine.Attack03State);
        }
        else if (attackTimeElapsed >= maxTimeToNextAttack && !canContinueCombo)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        canContinueCombo = false;
        attackFinish = false;
        base.Exit();
        StopAnimation(stateMachine.Player.PlayerAnimationData.Attack02ParameterHash);
    }

    protected override void FinishAttack()
    {
        //Animator animator = stateMachine.Player.AnimPlayer;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Debug.Log("Animación de Attack02 acabada");
            attackFinish = true;
        }
    }

    protected override void Move()
    {
        if (!attackFinish)
            return;
    }
}
