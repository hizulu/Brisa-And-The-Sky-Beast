using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Retreat-Run Away", menuName = "Enemy Logic/Retreat Logic/Run Away")]
public class EnemyRetreatRunAway : EnemyRetreatSOBase
{
    [SerializeField] private float runAwaySpeed = 3.5f;
    [SerializeField] private float playerChaseRange = 10f;
    [SerializeField] private float playerLostRange = 20f;

    private float playerChaseRangeSQR = 0f;
    private float playerLostRangeSQR = 0f;

    private bool hasRetreated = false;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        playerChaseRangeSQR = playerChaseRange * playerChaseRange;
        playerLostRangeSQR = playerLostRange * playerLostRange;

        hasRetreated = false;

        // Configurar velocidad del agente
        enemy.agent.speed = runAwaySpeed;
        enemy.agent.updateRotation = true;
        enemy.agent.updatePosition = true;

        SetRetreatDestination();
        Debug.Log("Entra en estado de huida");
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
            if (distanceToPlayerSQR > playerLostRangeSQR)
            {
                hasRetreated = true;
                Debug.Log("Ha huido con éxito");
            }
        }
        else
        {
            if (distanceToPlayerSQR < playerChaseRangeSQR)
            {
                Debug.Log("Volver a perseguir");
                enemy.doChase = true;
                enemy.doRetreat = false;
            }
            else
            {
                Debug.Log("Volver a Idle");
                enemy.doIdle = true;
                enemy.doRetreat = false;
            }
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        // No se necesita lógica física, ya que el NavMeshAgent se encarga del movimiento
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

    private void SetRetreatDestination()
    {
        Vector3 directionAway = (enemy.transform.position - playerTransform.position).normalized;
        Vector3 targetPos = enemy.transform.position + directionAway * 10f; // Puede ajustarse si quieres que se aleje más

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
        {
            enemy.agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("No se encontró un punto válido en el NavMesh para huir");
        }
    }
}
