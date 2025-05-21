using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * NOMBRE CLASE: EnemyRetreatRecalculateTarget
 * AUTOR: Jone Sainz Egea
 * FECHA: 21/05/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Retreat en el que el enemigo solo recalcula el objetivo.
 *              Para ello, vuelve al estado de Patrol, en el que está la lógica de detección de obejetivo.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de retirada del objetivo.
 */
[CreateAssetMenu(fileName = "Retreat-RecalculateTarget", menuName = "Enemy Logic/Retreat Logic/Recalculate Target")]
public class EnemyRetreatRecalculateTarget : EnemyStateSOBase
{
    [SerializeField] private float _timeBetweenHits = 2f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //Debug.Log("Entra en retreat");
        enemy.StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_timeBetweenHits);
        if(enemy.currentHealth > Mathf.Epsilon)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyPatrolState);
    }
}
