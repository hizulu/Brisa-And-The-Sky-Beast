using UnityEngine;

/*
 * NOMBRE CLASE: EnemyPatrolRandomWander
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Patrol en el que el enemigo se mueve a un punto aleatorio en un anillo definido a su alrededor.
 *              Se define el punto al que debe ir de forma aleatoria en un anillo a su alrededor definido por un radio mínimo y un radio máximo.
 *              Vuelve al estado de Idle cuando llega al destino. Cambia a estado de Chase si detecta al jugador.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de patrullar por puntos aleatorios.
 */
[CreateAssetMenu(fileName = "Patrol-Random Wander", menuName = "Enemy Logic/Patrol Logic/Random Wander")]
public class EnemyPatrolRandomWander : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float minRandomRadius = 5f;
    [SerializeField] private float maxRandomRadius = 12f;
    [SerializeField] private float randomWanderSpeed = 3f;
    [SerializeField] private float playerDetectionRange = 12f;

    private float playerDetectionRangeSQR = 0f; // Variable auxiliar para almacenar distancia evitando cálculo de raíz cuadrada cada frame.

    private Vector3 targetPos;
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        playerDetectionRangeSQR = playerDetectionRange * playerDetectionRange;

        targetPos = GetRandomPointInRingAroundEnemy();

        enemy.agent.speed = randomWanderSpeed;
        enemy.MoveEnemy(targetPos);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        enemy.agent.ResetPath();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        // Cambia de estado cuando llega al destino
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);

        // Cambia de estado cuando detecta al jugador
        PlayerDetection();
    }
    #endregion

    #region Métodos Específicos de EnemyPatrolRandomWander
    /*
     * Método que calcula el punto al que debe ir.
     * Primero calcula la distancia a la que está el punto de destino, después la dirección.
     * @return posición del punto aleatorio calculado
     */
    private Vector3 GetRandomPointInRingAroundEnemy()
    {
        float randomRadius = Random.Range(minRandomRadius, maxRandomRadius); // Distancia aleatoria dentro del anillo
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // Ángulo aleatorio en radianes

        // Cálculo de la dirección
        float offsetX = Mathf.Cos(randomAngle) * randomRadius;
        float offsetZ = Mathf.Sin(randomAngle) * randomRadius;

        // Devuelve la posición del punto al que debe ir
        return new Vector3(enemy.transform.position.x + offsetX, enemy.transform.position.y, enemy.transform.position.z + offsetZ);
    }

    /*
     * Método que revisa la distancia a la que se encuentra el jugador, si este se encuentra cerca cambia al estado de persecución
     */
    private void PlayerDetection()
    {
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;
        if (distanceToPlayerSQR < playerDetectionRangeSQR)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
    }
    #endregion
}
