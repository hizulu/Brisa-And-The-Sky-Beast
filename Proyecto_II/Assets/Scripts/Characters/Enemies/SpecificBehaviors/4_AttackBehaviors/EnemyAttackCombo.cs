using UnityEngine;

/*
 * NOMBRE CLASE: EnemyAttackCombo
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 06/05/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de ataque del Demonio del Aire.        
 * VERSIÓN: 1.0.
 */

[CreateAssetMenu(fileName = "Attack-Combo", menuName = "Enemy Logic/Attack Logic/Combo")]
public class EnemyAttackCombo : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private EnemyComboAttacksSOBase[] comboAttacks;

    private int currentAttack = 0;
    private bool isComboFinished = false;
    #endregion

    #region  Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        Debug.Log("El enemigo ha entrado en el estado de COMBO ATAQUE");
        base.DoEnterLogic();
        comboAttacks[0].Initialize(enemy, playerTransform, beastTransform);
        currentAttack = 0;
        isComboFinished = false;
        comboAttacks[currentAttack].EnemyAttack();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (isComboFinished)
        {
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);
            return;
        }

        comboAttacks[currentAttack].FrameLogic();

        if (comboAttacks[currentAttack].IsFinished())
        {
            comboAttacks[currentAttack].Exit();
            currentAttack++;

            if (currentAttack < comboAttacks.Length)
            {
                comboAttacks[currentAttack].Initialize(enemy, playerTransform, beastTransform);
                comboAttacks[currentAttack].EnemyAttack();
            }
            else
                isComboFinished = true;
        }
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("El enemigo ha salido del estado de COMBO ATAQUE");
    }
    #endregion
}