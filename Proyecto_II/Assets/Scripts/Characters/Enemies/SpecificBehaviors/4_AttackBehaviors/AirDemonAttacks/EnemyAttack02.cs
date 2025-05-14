using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttack02
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 07/05/2025
 * DESCRIPCIÓN: Clase que define el segundo ataque del combo del demonio de aire   
 * VERSIÓN: 1.0.
 */

[CreateAssetMenu(fileName = "Combo - Attack02", menuName = "Enemy Logic/Attack Logic/Attack02")]
public class EnemyAttack02 : EnemyComboAttacksSOBase
{
    #region Variables
    [SerializeField] private float attackDamageModifierMin = 20f;
    [SerializeField] private float attackDamageModifierMax = 30f;
    bool attack02 = false;
    #endregion

    #region Sobreescriturta de métodos de EnemyComboAttacksSOBase
    public override void Initialize(Enemy _enemy)
    {
        base.Initialize(_enemy);
        attack02 = false;
        enemy.anim.SetBool("isAttacking02", true);
        float randomAttackDamage02 = Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        AttackTarget(randomAttackDamage02);
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
    #endregion
}