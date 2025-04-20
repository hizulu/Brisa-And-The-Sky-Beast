using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Retreat-Run Away", menuName = "Enemy Logic/Retreat Logic/Run Away")]
public class EnemyRetreatRunAway : EnemyRetreatSOBase
{
    [SerializeField] private float runAwaySpeed = 3.5f;
    [SerializeField] private float runAwayDistance = 10f;
    [SerializeField] private float playerChaseRange = 10f;

    private float playerChaseRangeSQR = 0f;

    private Vector3 positionToRetreatTo; // Posición inicial a la que tiene que huír

    private bool hasRetreated = false;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        playerChaseRangeSQR = playerChaseRange * playerChaseRange;

        hasRetreated = false;


        enemy.agent.speed = runAwaySpeed;

        positionToRetreatTo = SetRetreatDestination();

        enemy.MoveEnemy(positionToRetreatTo);

        //Debug.Log("Entra en estado de huida");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.ResetPath();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (!hasRetreated)
        {
            if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending)
            {
                hasRetreated = true;
                //Debug.Log("Ha huido con éxito");
            }
        }
        else
        {
            if (distanceToPlayerSQR < playerChaseRangeSQR)
            {
                //Debug.Log("Volver a perseguir");
                enemy.doChase = true;
                enemy.doRetreat = false;
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyChaseState);
            }
            else
            {
                //Debug.Log("Volver a Idle");
                enemy.doIdle = true;
                enemy.doRetreat = false;
                enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyIdleState);
            }
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
        hasRetreated = false;
    }

    private Vector3 SetRetreatDestination()
    {
        Vector3 directionAway = (enemy.transform.position - playerTransform.position).normalized;
        Vector3 targetPos = enemy.transform.position + directionAway * runAwayDistance; 

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning("No se encontró un punto válido en el NavMesh para huir");
            return Vector3.zero;
        }
    }
}
