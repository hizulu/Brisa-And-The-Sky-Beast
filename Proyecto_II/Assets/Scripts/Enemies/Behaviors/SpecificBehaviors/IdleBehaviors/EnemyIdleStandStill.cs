using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Stand Still", menuName = "Enemy Logic/Idle Logic/Stand Still")]
public class EnemyIdleStandStill : EnemyIdleSOBase
{
    [SerializeField] float minStillTime = 1f;
    [SerializeField] float maxStillTime = 5f;
    private float stillTime;
    
    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Has entrado en estado de Idle - Stand Still");
        //enemy.anim.SetBool("isIdling", true);

        stillTime = Random.Range(minStillTime, maxStillTime);
        Debug.Log($"Time to stay still: {stillTime}");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Debug.Log("Has salido del estado de Idle - Stand Still");
        //enemy.anim.SetBool("isIdling", false);
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
        }
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
