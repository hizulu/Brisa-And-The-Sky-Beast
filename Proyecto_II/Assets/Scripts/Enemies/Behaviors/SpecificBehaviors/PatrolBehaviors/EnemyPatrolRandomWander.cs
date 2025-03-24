using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol-Random Wander", menuName = "Enemy Logic/Patrol Logic/Random Wander")]
public class EnemyPatrolRandomWander : EnemyPatrolSOBase
{
    [SerializeField] private float minRandomRadius = 5f;
    [SerializeField] private float maxRandomRadius = 12f;
    [SerializeField] private float randomMovementSpeed = 1f;

    private Vector3 targetPos;
    private Vector3 direction;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        targetPos = GetRandomPointInRingAroundEnemy();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        direction = (targetPos - enemy.transform.position).normalized;

        enemy.MoveEnemy(direction * randomMovementSpeed);

        if ((enemy.transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            enemy.doIdle = true;
            enemy.doPatrol = false;
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

    private Vector3 GetRandomPointInRingAroundEnemy()
    {
        Debug.Log("Nuevo punto en la rosca");

        float randomRadius = Random.Range(minRandomRadius, maxRandomRadius);
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // Ángulo aleatorio en radianes

        float offsetX = Mathf.Cos(randomAngle) * randomRadius;
        float offsetZ = Mathf.Sin(randomAngle) * randomRadius;

        return new Vector3(enemy.transform.position.x + offsetX, enemy.transform.position.y, enemy.transform.position.z + offsetZ);
    }
}
