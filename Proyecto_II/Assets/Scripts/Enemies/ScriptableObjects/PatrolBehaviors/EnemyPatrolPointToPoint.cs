using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol-Point to Point", menuName = "Enemy Logic/Patrol Logic/Point to Point")]
public class EnemyPatrolPointToPoint : EnemyPatrolSOBase
{
    #region Variables
    [SerializeField] private float PointToPointMovementSpeed = 1f;
    [SerializeField] private float playerDetectionRange = 12f;
    [SerializeField] private float randomIdle = 0.3f;

    private List<Transform> patrolPoints = new List<Transform>(); // Lista para guardar los puntos a los que deben ir los enemigos (el recorrido de patrulla).

    private int currentPoint = 0; // Guardar el punto en el que est�n.

    private Vector3 _targetPos;
    private Vector3 _direction;
    private float playerDetectionRangeSQR = 0f;
    private int lastPointSaved = 0;
    #endregion

    #region M�todos de EnemyPatrolSOBase
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        playerDetectionRangeSQR = playerDetectionRange * playerDetectionRange;
        //Debug.Log("Has entrado en estado de PatrolPointToPoint");
        AddPatrolPoints();
        ReturnFromIdle();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        //Debug.Log("Has salido del estado Patrol - Point To Point");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        ChangePoint();
        PlayerDetected();
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        SetEnemyMovement();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
    #endregion

    #region M�todos Espec�ficos de EnemyPatrolPointToPoint
    /*
     * M�todo para obtener la direcci�n que deben seguir los enemigos hasta los puntos de patrullaje.
     * La direcci�n obtenida se env�a al m�todo MoveEnemy del script: "Enemy".
     */
    private void SetEnemyMovement()
    {
        _direction = (_targetPos - enemy.transform.position).normalized;

        enemy.MoveEnemy(_direction * PointToPointMovementSpeed);

        if (_direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    /*
     * M�todo donde se busca un objeto llamado "PatrolPoints" que guarda los puntos que debe recorrer cada enemigo.
     * Cada enemigo puede tener sus propios puntos (pueden ser m�s o menos y en diferentes sitios).
     */
    private void AddPatrolPoints()
    {
        Transform parent = transform.parent.Find("PatrolPoints");

        if (parent != null)
        {
            patrolPoints.Clear(); // Limpiar la lista antes de comenzar a a�adirlos para que no de problemas de otras llamadas.

            foreach (Transform child in parent)
                patrolPoints.Add(child); // A�adimos a la lista los puntos hijos.

            if (patrolPoints.Count > 0) // Comprobamos que la lista no est� vac�a.
                _targetPos = patrolPoints[0].position; // Asignamos que la posici�n a la que debe ir primero es el primer elemento de la lista (Punto1).
        }
    }

    /*
     * M�todo para que el enemigo vaya cambiando de punto.
     */
    private void ChangePoint()
    {
        if ((enemy.transform.position - _targetPos).sqrMagnitude < 2f) //Comprueba que la distancia entre el enemigo y el punto al que debe ir sea menor que 2.
        {
            if (!enemy.doIdle && Random.value < randomIdle)
            {
                lastPointSaved = currentPoint; // Guardar el punto actual antes de pasar a IdleState.
                enemy.doIdle = true;
                enemy.doPatrol = false;
            }
            else
            {
                //Debug.Log($"Lleg� al punto {currentPoint}: {patrolPoints[currentPoint].name}");
                currentPoint = (currentPoint + 1) % patrolPoints.Count; // El punto actual se actualiza al siguiente de la lista y si es el �ltimo punto de todos, reinicia el �ndice.
                _targetPos = patrolPoints[currentPoint].position; // Asigna la posici�n a la que debe ir el enemigo al punto actual de la lista.
            }                
        }
    }

    /*
     * M�todo que guarda el �ltimo punto en el que est� el enemigo para no reiniciar la lista de nuevo, sino que siga el orden establecido.
     */
    public void ReturnFromIdle()
    {
        currentPoint = lastPointSaved; // Asignar el �ltimo punto guardado como el actual.
        _targetPos = patrolPoints[currentPoint].position;
    }

    private void PlayerDetected()
    {
        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR < playerDetectionRangeSQR)
        {
            Debug.Log("Deber�a perseguir a Brisa");
            enemy.doChase = true;
            enemy.doPatrol = false;
        }
    }
    #endregion
}
