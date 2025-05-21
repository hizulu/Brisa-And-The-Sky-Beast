using System.Collections.Generic;
using UnityEngine;

/*
 * NOMBRE CLASE: EnemyPatrolPointToPoint
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 02/04/2025
 * DESCRIPCIÓN: Clase que define el comportamiento específico de Patrol en el que el enemigo se mueve de punto a punto.
 *              Los puntos  de patrulla se definen como hijos del objeto que contiene el script de Enemy.
 *              Vuelve al estado de Idle cuando llega al destino. Cambia a estado de Chase si detecta al objetivo.
 *              Hereda de EnemyStateSOBase, por lo que se crea desde el editor de Unity. Sobreescribe sus métodos y tiene acceso a sus variables.            
 * VERSIÓN: 1.0. Script base con el comportamiento de patrullar de punto a punto.
 *              1.1. Añadido detección de Bestia (22/04/2025) - Jone
 */
[CreateAssetMenu(fileName = "Patrol-Point to Point", menuName = "Enemy Logic/Patrol Logic/Point to Point")]
public class EnemyPatrolPointToPoint : EnemyStateSOBase
{
    #region Variables
    [SerializeField] private float PointToPointMovementSpeed = 1f;
    // [SerializeField] private float playerDetectionRange = 20f;
    [SerializeField] private float randomIdle = 0.3f;

    private List<Transform> patrolPoints = new List<Transform>(); // Lista para guardar los puntos a los que deben ir los enemigos (el recorrido de patrulla).

    private int currentPoint = 0; // Guardar el punto en el que están.
    private int lastPointSaved = 0;

    [SerializeField] private EnemyTargetDetectionSOBase targetDetection;

    private Vector3 _targetPos;
    #endregion

    #region Sobreescriturta de métodos de EnemyStateSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //Debug.Log("Entra en patrol");

        enemy.anim.SetBool("isPatrol", true);

        targetDetection.Initialize(enemy);

        AddPatrolPoints();
        ReturnFromIdle();

        enemy.agent.speed = PointToPointMovementSpeed;
        enemy.MoveEnemy(_targetPos);
    }

    public override void DoFrameUpdateLogic()
    {
        enemy.SfxEnemy.PlayRandomSFX(EnemySFXType.Walk);

        base.DoFrameUpdateLogic();
        ChangePoint();

        // Cambia de estado cuando detecta al jugador o a la bestia
        if (targetDetection.LookForTarget())
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
    }

    public override void DoExitLogic()
    {
        enemy.SfxEnemy.StopSound(EnemySFXType.Walk);
        base.DoExitLogic();
        enemy.anim.SetBool("isPatrol", false);
    }
    #endregion

    #region Métodos Específicos de EnemyPatrolPointToPoint
    /*
     * Método donde se busca un objeto llamado "PatrolPoints" que guarda los puntos que debe recorrer cada enemigo.
     * Cada enemigo puede tener sus propios puntos (pueden ser más o menos y en diferentes sitios).
     */
    private void AddPatrolPoints()
    {
        Transform parent = transform.parent.Find("PatrolPoints");

        if (parent != null)
        {
            patrolPoints.Clear(); // Limpiar la lista antes de comenzar a añadirlos para que no de problemas de otras llamadas.

            foreach (Transform child in parent)
                patrolPoints.Add(child); // Añadimos a la lista los puntos hijos.

            if (patrolPoints.Count > 0) // Comprobamos que la lista no esté vacía.
                _targetPos = patrolPoints[0].position; // Asignamos que la posición a la que debe ir primero es el primer elemento de la lista (Punto1).
        }
    }

    /*
     * Método para que el enemigo vaya cambiando de punto.
     */
    private void ChangePoint()
    {
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending) // Comprueba que el enemigo llegue al destino.
        {
            if (Random.value < randomIdle)
            {
                lastPointSaved = currentPoint; // Guardar el punto actual antes de pasar a IdleState.
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
            }
            else
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Count; // El punto actual se actualiza al siguiente de la lista y si es el último punto de todos, reinicia el índice.
                _targetPos = patrolPoints[currentPoint].position; // Asigna la posición a la que debe ir el enemigo al punto actual de la lista.
                enemy.MoveEnemy(_targetPos);
            }                
        }
    }

    /*
     * Método que guarda el último punto en el que está el enemigo para no reiniciar la lista de nuevo, sino que siga el orden establecido.
     */
    public void ReturnFromIdle()
    {
        currentPoint = lastPointSaved; // Asignar el último punto guardado como el actual.
        _targetPos = patrolPoints[currentPoint].position;
    }
    #endregion
}
