using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttack02
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 07/05/2025
 * DESCRIPCI�N: Clase que define el segundo ataque del combo del demonio de aire   
 * VERSI�N: 1.0.
 */

[CreateAssetMenu(fileName = "Combo - Attack02", menuName = "Enemy Logic/Attack Logic/Attack02")]
public class EnemyAttack02 : EnemyComboAttacksSOBase
{
    #region Variables
    [SerializeField] private float attackDamageModifierMin = 20f;
    [SerializeField] private float attackDamageModifierMax = 30f;
    bool attack02 = false;
    [SerializeField] private float distanceToHit = 5f;
    #endregion

    #region Sobreescriturta de m�todos de EnemyComboAttacksSOBase
    public override void Initialize(Enemy _enemy, Transform _playerTransform, Transform _beastTransform)
    {
        base.Initialize(_enemy, _playerTransform, _beastTransform);
        attack02 = false;
        enemy.anim.SetTrigger("Attack02");
        float randomAttackDamage02 = Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        AttackTarget(randomAttackDamage02, distanceToHit);
        enemy.StartCoroutine(WaitForAnimation());
        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Attack);
    }

    public override void Exit()
    {
        Debug.Log("Has salido del segundo ataque-");
        base.Exit();
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
            Debug.Log("Finished animation2");
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        attack02 = true;
    }

    public override bool IsFinished()
    {
        return attack02;
    }
    #endregion
}