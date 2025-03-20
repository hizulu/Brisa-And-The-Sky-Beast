using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Stand Still", menuName = "Enemy Logic/Idle Logic/Stand Still")]
public class EnemyIdleStandStill : EnemyIdleSOBase
{
    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        Debug.Log("Estás en el script de StandStill");
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Has entrado en estado de IDLESSTILL");
        //enemy.anim.SetBool("isIdling", true);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Estás en IDLESSTILL");
        //enemy.anim.SetBool("isIdling", false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
    }

    public override void DoPhysiscsLogic()
    {
        base.DoPhysiscsLogic();
    }    

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
