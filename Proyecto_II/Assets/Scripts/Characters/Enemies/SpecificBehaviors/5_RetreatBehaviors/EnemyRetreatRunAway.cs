using UnityEngine;
using UnityEngine.AI;

/*
 * NOMBRE CLASE: EnemyRetreatRunAway
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Retreat en el que el enemigo huye en la dirección contraria del objetivo.
 *              Después de huir, cambia a estado de Chase si el objetivo se encuentra lo suficientemente cerca para seguirlo.
 *              Después de huir, si el objetivo se encuentra lejos, vuelve a estado de Idle.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de huir del jugador.
 *              1.1. Añadido huir de la bestia (22/04/2025)
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
    private Vector3 positionToRetreatTo; // Posición inicial a la que tiene que huír

    private bool hasRetreated = false;
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
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
            // Comprobación de que el enemigo haya huído con éxito
            if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending)
                hasRetreated = true;
        }
        // Cuando el enemigo ya ha huído
        else
        {
            // Si el objetivo se encuentra en rango de persecución, vuelve al estado de Chase
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

    #region Métodos Específicos de EnemyRetreatRunAway
    /*
     * Método que establece el objetivo del que el enemigo debe huir según la variable de enemy
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
     * Método que calcula la posición a la que debe huír el enemigo
     * @return Vector3 de posición a la que debe huír
     */
    private Vector3 SetRetreatDestination()
    {
        // Cálculo de la posición a la que debe huír
        Vector3 directionAway = (enemy.transform.position - targetTransform.position).normalized;
        Vector3 targetPos = enemy.transform.position + directionAway * runAwayDistance; 

        // Comprobación de que el enemigo pueda dirigirse a esa posición con el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 15f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning("No se encontró un punto válido en el NavMesh para huir");
            return Vector3.zero;
        }
    }
    #endregion
}
