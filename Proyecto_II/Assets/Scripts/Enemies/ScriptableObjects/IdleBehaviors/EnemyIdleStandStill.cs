using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Stand Still", menuName = "Enemy Logic/Idle Logic/Stand Still")]
public class EnemyIdleStandStill : EnemyIdleSOBase
{
    [SerializeField] float minStillTime = 1f;
    [SerializeField] float maxStillTime = 5f;

    private float stillTime;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        stillTime = Random.Range(minStillTime, maxStillTime);
        Debug.Log($"Time to stay still: {stillTime}");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        stillTime -= Time.deltaTime;

        if (stillTime <= 0)
        {
            Debug.Log("Finished idle time.");
            enemy.doIdle = false;
            enemy.doPatrol = true;
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyPatrolState);
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
    }
}
