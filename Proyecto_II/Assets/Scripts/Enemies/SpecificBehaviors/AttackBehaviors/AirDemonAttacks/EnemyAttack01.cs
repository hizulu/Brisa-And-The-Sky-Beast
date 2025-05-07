using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo - Attack01", menuName = "Enemy Logic/Attack Logic/Attack01")]
public class EnemyAttack01 : EnemyComboAttacksSOBase
{
    bool attack01 = false;

    public override void Initialize(Enemy _enemy)
    {
        base.Initialize(_enemy);
        attack01 = false;
        enemy.anim.SetBool("isAttacking01", true);
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

    public override void EnemyAttack()
    {
        Debug.Log("El enemigo ha hecho el primer ataque del combo.");
    }

    public override void FinishAnimation()
    {

            Debug.Log("Current anim state: " + enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"));

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