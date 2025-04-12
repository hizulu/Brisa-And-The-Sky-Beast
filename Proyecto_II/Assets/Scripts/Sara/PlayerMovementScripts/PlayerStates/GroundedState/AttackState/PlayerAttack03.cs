using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack03 : PlayerAttackState
{
    public PlayerAttack03(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    //public static event Action<float> OnAttack03Enemy;

    public override void Enter()
    {
        attackFinish = false;
        attackDamageModifier = 2;
        base.Enter();
        stateMachine.Player.GolpearPrueba();
        StartAnimation(stateMachine.Player.PlayerAnimationData.Attack03ParameterHash);

        float attackDamageCombo03 = stateMachine.StatsData.AttackDamageBase * attackDamageModifier;
        EventsManager.TriggerSpecialEvent<float>("OnAttack03Enemy", attackDamageCombo03);
        //OnAttack03Enemy?.Invoke(attackDamageCombo03);
        Debug.Log("Daño del ataque 3: " + " " + attackDamageCombo03);
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
