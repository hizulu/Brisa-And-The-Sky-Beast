using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Stand Still", menuName = "Enemy Logic/Idle Logic/Stand Still")]
public class EnemyIdleStandStill : EnemyIdleSOBase
{
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        enemy.anim.SetBool("seMueve", false);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        enemy.anim.SetBool("seMueve", true);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
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
}
