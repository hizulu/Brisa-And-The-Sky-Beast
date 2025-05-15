using System.Collections;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttack01
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 07/05/2025
 * DESCRIPCIÓN: Clase que define el primer ataque del combo del demonio de aire        
 * VERSIÓN: 1.0.
 */

[CreateAssetMenu(fileName = "Combo - Attack01", menuName = "Enemy Logic/Attack Logic/Attack01")]
public class EnemyAttack01 : EnemyComboAttacksSOBase
{
    #region Variables   
    bool attack01 = false;
    [SerializeField] private float attackDamageModifierMin = 15f;
    [SerializeField] private float attackDamageModifierMax = 20f;
    [SerializeField] private float distanceToHit = 3f;
    #endregion

    #region Sobreescriturta de métodos de EnemyComboAttacksSOBase
    public override void Initialize(Enemy _enemy, Transform _playerTransform, Transform _beastTransform)
    {
        base.Initialize(_enemy, _playerTransform, _beastTransform);
        attack01 = false;
        enemy.anim.SetTrigger("Attack01");
        float randomAttackDamage01 = Random.Range(attackDamageModifierMin, attackDamageModifierMax);
        AttackTarget(randomAttackDamage01, distanceToHit); // Método definido en el SOBase
        enemy.StartCoroutine(WaitForAnimation());
    }

    public override void Exit()
    {
        Debug.Log("Has salido del primer ataque-");
        base.Exit();
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

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        attack01 = true;
    }

    public override bool IsFinished()
    {
        return attack01;
    }
    #endregion
}