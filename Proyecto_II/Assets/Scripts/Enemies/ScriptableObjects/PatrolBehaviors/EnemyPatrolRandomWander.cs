using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 24/03/2025
[CreateAssetMenu(fileName = "Patrol-Random Wander", menuName = "Enemy Logic/Patrol Logic/Random Wander")]
public class EnemyPatrolRandomWander : EnemyPatrolSOBase
{
    [SerializeField] private float minRandomRadius = 5f;
    [SerializeField] private float maxRandomRadius = 12f;
    [SerializeField] private float randomWanderSpeed = 3f;
    [SerializeField] private float playerDetectionRange = 12f;

    private float playerDetectionRangeSQR = 0f;

    private Vector3 targetPos;
    private Vector3 direction;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        playerDetectionRangeSQR = playerDetectionRange * playerDetectionRange;
        targetPos = GetRandomPointInRingAroundEnemy();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();       

        if ((enemy.transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            enemy.doIdle = true;
            enemy.doPatrol = false;
        }

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;
        
        if (distanceToPlayerSQR < playerDetectionRangeSQR)
        {
            //Debug.Log("Deber�a perseguir a Brisa");
            enemy.doChase = true;
            enemy.doPatrol = false;
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();

        direction = (targetPos - enemy.transform.position).normalized;

        enemy.MoveEnemy(direction * randomWanderSpeed);
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
        //Debug.Log("Nuevo punto en la rosca");

        float randomRadius = Random.Range(minRandomRadius, maxRandomRadius);
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // �ngulo aleatorio en radianes

        float offsetX = Mathf.Cos(randomAngle) * randomRadius;
        float offsetZ = Mathf.Sin(randomAngle) * randomRadius;

        return new Vector3(enemy.transform.position.x + offsetX, enemy.transform.position.y, enemy.transform.position.z + offsetZ);
    }
}
