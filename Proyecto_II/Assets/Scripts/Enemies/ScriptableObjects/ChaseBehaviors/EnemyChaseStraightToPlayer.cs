using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Straight to Player", menuName = "Enemy Logic/Chase Logic/Straight to Player")]
public class EnemyChaseStraightToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float chasingSpeed = 4f;
    private Vector3 direction;
    [SerializeField] private float playerAttackRange = 7f;
    [SerializeField] private float playerLostRange = 20f;

    private float playerAttackRangeSQR = 0f;
    private float playerLostRangeSQR = 0f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        playerAttackRangeSQR = playerAttackRange * playerAttackRange;
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

        if (distanceToPlayerSQR < playerAttackRangeSQR)
        {
            Debug.Log("Debería atacar a Brisa");
            enemy.doAttack = true;
            enemy.doChase = false;
        }
        else if (distanceToPlayerSQR > playerLostRangeSQR)
        {
            Debug.Log("Debería volver a idle");
            enemy.doIdle = true;
            enemy.doChase = false;
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();

        direction = (enemy.transform.position - playerTransform.position).normalized;

        enemy.MoveEnemy(direction * chasingSpeed);
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

