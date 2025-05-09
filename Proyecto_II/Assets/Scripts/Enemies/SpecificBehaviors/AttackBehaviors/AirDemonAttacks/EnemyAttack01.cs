using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttack01
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 07/05/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Retreat       
 * VERSIÓN: 1.0.
 */

[CreateAssetMenu(fileName = "Combo - Attack01", menuName = "Enemy Logic/Attack Logic/Attack01")]
public class EnemyAttack01 : EnemyComboAttacksSOBase
{
    bool attack01 = false;
    [SerializeField] private float attackDamageModifierMin = 15f;
    [SerializeField] private float attackDamageModifierMax = 20f;

    public override void Initialize(Enemy _enemy)
    {
        base.Initialize(_enemy);
        attack01 = false;
        enemy.anim.SetBool("isAttacking01", true);
        float randomAttackDamage01 = UnityEngine.Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        EventsManager.TriggerSpecialEvent<float>("OnAttackPlayer", randomAttackDamage01);
    }

    public override void Exit()
    {
        Debug.Log("Has salido del primer ataque-");
        base.Exit();
        enemy.anim.SetBool("isAttacking01", false);
    }

    public override void FrameLogic()
    {
        base.FrameLogic();
        FinishAnimation();
    }

    public override void FinishAnimation()
    {
        if (enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            attack01 = true;
        }
    }

    public override bool IsFinished()
    {
        return attack01;
    }

}