using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Retreat-Run Away", menuName = "Enemy Logic/Retreat Logic/Run Away")]
public class EnemyRetreatRunAway : EnemyRetreatSOBase
{
    [SerializeField] private float runAwaySpeed = 1.5f;
    [SerializeField] private float playerChaseRange = 10f;
    [SerializeField] private float playerLostRange = 20f;

    private Vector3 direction;

    private float playerChaseRangeSQR = 0f;
    private float playerLostRangeSQR = 0f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        playerChaseRangeSQR = playerChaseRange * playerChaseRange;
        playerLostRangeSQR = playerLostRange * playerLostRange;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR < playerChaseRange)
        {
            Debug.Log("Debería volver a chase");
            enemy.doChase = true;
            enemy.doRetreat = false;
        }
        else if (distanceToPlayerSQR > playerLostRangeSQR)
        {
            Debug.Log("Debería volver a idle");
            enemy.doIdle = true;
            enemy.doRetreat = false;
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();

        direction = (enemy.transform.position - playerTransform.position).normalized;
        direction.y = 0;
        enemy.MoveEnemy(direction * runAwaySpeed);
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
