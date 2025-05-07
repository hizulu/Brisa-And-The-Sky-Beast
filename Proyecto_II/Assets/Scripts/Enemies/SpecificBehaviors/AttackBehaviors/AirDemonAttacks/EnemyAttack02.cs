using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo - Attack02", menuName = "Enemy Logic/Attack Logic/Attack02")]
public class EnemyAttack02 : EnemyComboAttacksSOBase
{
    bool attack02 = false;

    public override void Initialize(Enemy _enemy)
    {
        base.Initialize(_enemy);
        attack02 = false;
        enemy.anim.SetBool("isAttacking02", true);
    }

    public override void Exit()
    {
        Debug.Log("Has salido del segundo ataque-");
        base.Exit();
        enemy.anim.SetBool("isAttacking02", false);
    }

    public override void FrameLogic()
    {
        base.FrameLogic();
        FinishAnimation();
    }

    public override void EnemyAttack()
    {
        Debug.Log("El enemigo ha hecho el segundo ataque del combo.");
    }

    public override void FinishAnimation()
    {
        

        if (enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            attack02 = true;
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);
        }
    }

    public override bool IsFinished()
    {
        return attack02;
    }
}