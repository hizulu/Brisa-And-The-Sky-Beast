using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol-Random Wander", menuName = "Enemy Logic/Patrol Logic/Random Wander")]
public class EnemyPatrolRandomWander : EnemyPatrolSOBase
{
    [SerializeField] private float randomMovementRange = 2f;
    [SerializeField] private float randomMovementSpeed = 1f;

    [SerializeField] private float stuckTimeThreshold = 1f;

    private Vector3 targetPos;
    private Vector3 direction;

    private Vector3 lastPosition;
    private float stuckTimer;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        lastPosition = enemy.transform.position;
        stuckTimer = 0f;
        targetPos = GetRandomPointInCircle();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        direction = (targetPos - enemy.transform.position).normalized;

        //enemy.MoveEnemy(direction * randomMovementSpeed);

        if ((enemy.transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            targetPos = GetRandomPointInCircle();
        }

        // Comprueba si la posición del enemigo ha cambiado
        if (Vector3.Distance(enemy.transform.position, lastPosition) < 0.02f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        // Actualiza la última posición para la comprobación de movimiento
        lastPosition = enemy.transform.position;

        // Si el enemigo ha estado atascado más del tiempo permitido, vuelve al estado de idle
        if (stuckTimer >= stuckTimeThreshold)
        {
            targetPos = GetRandomPointInCircle();
        }
    }

    public override void DoPhysiscsLogic()
    {
        base.DoPhysiscsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private Vector3 GetRandomPointInCircle()
    {
        return enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * randomMovementRange;
    }
}
