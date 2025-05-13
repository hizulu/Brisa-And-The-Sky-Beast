using UnityEngine;
using UnityEngine.AI;

/*
 * NOMBRE CLASE: EnemyRetreatRunAway
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCI�N: Clase que define el comportamiento espec�fico de Retreat en el que el enemigo huye en la direcci�n contraria del objetivo.
 *              Despu�s de huir, cambia a estado de Chase si el objetivo se encuentra lo suficientemente cerca para seguirlo.
 *              Despu�s de huir, si el objetivo se encuentra lejos, vuelve a estado de Idle.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.            
 * VERSI�N: 1.0. Script base con el comportamiento de huir del jugador.
 *              1.1. A�adido huir de la bestia (22/04/2025)
 */
[CreateAssetMenu(fileName = "Retreat-Run Away", menuName = "Enemy Logic/Retreat Logic/Run Away")]
public class EnemyRetreatRunAway : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float runAwaySpeed = 3.5f;
    [SerializeField] private float runAwayDistance = 10f;
    [SerializeField] private float targetChaseRange = 10f;

    private float targetChaseRangeSQR = 0f;

    private Transform targetTransform;
    private Vector3 positionToRetreatTo; // Posici�n inicial a la que tiene que hu�r

    private bool hasRetreated = false;
    #endregion

    #region Sobreescriturta de m�todos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        targetChaseRangeSQR = targetChaseRange * targetChaseRange;

        hasRetreated = false;

        SetTarget();


        enemy.agent.speed = runAwaySpeed;

        positionToRetreatTo = SetRetreatDestination();

        enemy.MoveEnemy(positionToRetreatTo);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.ResetPath();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToTargetSQR = (enemy.transform.position - targetTransform.position).sqrMagnitude;

        if (!hasRetreated)
        {
            // Comprobaci�n de que el enemigo haya hu�do con �xito
            if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending)
                hasRetreated = true;
        }
        // Cuando el enemigo ya ha hu�do
        else
        {
            // Si el objetivo se encuentra en rango de persecuci�n, vuelve al estado de Chase
            if (distanceToTargetSQR < targetChaseRangeSQR)
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
            // Si no, vuelve al estado de Idle
            else
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        hasRetreated = false;
    }
    #endregion

    #region M�todos Espec�ficos de EnemyRetreatRunAway
    /*
     * M�todo que establece el objetivo del que el enemigo debe huir seg�n la variable de enemy
     * El objetivo se ha establecido anteriormente en el estado de Patrol
     */
    private void SetTarget()
    {
        if (enemy.targetIsPlayer)
        {
            targetTransform = playerTransform;
        }
        else
        {
            targetTransform = beastTransform;
        }
    }
    /*
     * M�todo que calcula la posici�n a la que debe hu�r el enemigo
     * @return Vector3 de posici�n a la que debe hu�r
     */
    private Vector3 SetRetreatDestination()
    {
        // C�lculo de la posici�n a la que debe hu�r
        Vector3 directionAway = (enemy.transform.position - targetTransform.position).normalized;
        Vector3 targetPos = enemy.transform.position + directionAway * runAwayDistance; 

        // Comprobaci�n de que el enemigo pueda dirigirse a esa posici�n con el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 15f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning("No se encontr� un punto v�lido en el NavMesh para huir");
            return Vector3.zero;
        }
    }
    #endregion
}
