using UnityEngine;

/*
 * NOMBRE CLASE: EnemyPatrolRandomWander
 * AUTOR: Jone Sainz Egea
 * FECHA: 24/03/2025
 * DESCRIPCI�N: Clase que define el comportamiento espec�fico de Patrol en el que el enemigo se mueve a un punto aleatorio en un anillo definido a su alrededor.
 *              Se define el punto al que debe ir de forma aleatoria en un anillo a su alrededor definido por un radio m�nimo y un radio m�ximo.
 *              Vuelve al estado de Idle cuando llega al destino. Cambia a estado de Chase si detecta al jugador.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus m�todos y tiene acceso a sus variables.            
 * VERSI�N: 1.0. Script base con el comportamiento de patrullar por puntos aleatorios.
 *              1.1. A�adido detecci�n de Bestia (22/04/2025)
 */
[CreateAssetMenu(fileName = "Patrol-Random Wander", menuName = "Enemy Logic/Patrol Logic/Random Wander")]
public class EnemyPatrolRandomWander : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float minRandomRadius = 5f;
    [SerializeField] private float maxRandomRadius = 12f;
    [SerializeField] private float randomWanderSpeed = 3f;
    // [SerializeField] private float playerDetectionRange = 12f;

    [SerializeField] private EnemyTargetDetectionSOBase targetDetection; // Comportamiento serializado de detecci�n de objetivos

    private Vector3 targetPos;
    #endregion

    #region Sobreescriturta de m�todos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        targetDetection.Initialize(enemy);

        targetPos = GetRandomPointInRingAroundEnemy();

        enemy.agent.speed = randomWanderSpeed;
        enemy.MoveEnemy(targetPos);
    }

    public override void DoExitLogic()
    {
        enemy.SfxEnemy.StopSound(EnemySFXType.Idle);
        base.DoExitLogic();

        enemy.agent.ResetPath();
    }

    public override void DoFrameUpdateLogic()
    {
        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Walk);
        base.DoFrameUpdateLogic();

        // Cambia de estado cuando detecta al jugador o a la bestia
        if (targetDetection.LookForTarget())
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);

        // Cambia de estado cuando llega al destino
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending)
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
    }
    #endregion

    #region M�todos Espec�ficos de EnemyPatrolRandomWander
    /*
     * M�todo que calcula el punto al que debe ir.
     * Primero calcula la distancia a la que est� el punto de destino, despu�s la direcci�n.
     * @return posici�n del punto aleatorio calculado
     */
    private Vector3 GetRandomPointInRingAroundEnemy()
    {
        float randomRadius = Random.Range(minRandomRadius, maxRandomRadius); // Distancia aleatoria dentro del anillo
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // �ngulo aleatorio en radianes

        // C�lculo de la direcci�n
        float offsetX = Mathf.Cos(randomAngle) * randomRadius;
        float offsetZ = Mathf.Sin(randomAngle) * randomRadius;

        // Devuelve la posici�n del punto al que debe ir
        return new Vector3(enemy.transform.position.x + offsetX, enemy.transform.position.y, enemy.transform.position.z + offsetZ);
    }
    #endregion
}
