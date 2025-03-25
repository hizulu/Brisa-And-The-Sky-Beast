using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Retreat-Run Away", menuName = "Enemy Logic/Retreat Logic/Run Away")]
public class EnemyRetreatRunAway : EnemyRetreatSOBase
{
    //[SerializeField] private float _runAwaySpeed = 1.5f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //Vector2 runDir = -(playerTransform.position - transform.position).normalized;
        //enemy.MoveEnemy(runDir * _runAwaySpeed);

        //if (enemy.IsAggroed)
        //    enemy.PlaySonidosEnem(0); // Sonido idle
        //else
        //    SFXManager.instance.StopSFXLoop();
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
