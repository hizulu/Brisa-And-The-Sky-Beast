using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;

    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void DoEnterLogic() { }

    public virtual void DoExitLogic()
    {
        ResetValues();
    }

    public virtual void DoFrameUpdateLogic() { }

    public virtual void DoPhysiscsLogic() { }

    public virtual void ResetValues() { }

    protected virtual void MoveEnemy()
    {
        float speed = 5f;
        Vector3 movementDirection = transform.forward;
        transform.position += movementDirection * speed * Time.deltaTime;
    }
}
