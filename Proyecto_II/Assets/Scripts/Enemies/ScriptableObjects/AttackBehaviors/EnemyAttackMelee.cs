using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Melee", menuName = "Enemy Logic/Attack Logic/Melee")]
public class EnemyAttackMelee : EnemyAttackSOBase
{
    [SerializeField] private float _timeBetweenHits = 2f;
    [SerializeField] private float _attackDamage = 20f;

    private float _timer;

    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float distanceToStopAttackState = 5f;
    private float distanceToStopAttackStateSQR = 0f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.anim.SetBool("isAttacking", true);
        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;
        //Debug.Log("Has entrado en el estado de Attack Melee");
        enemy.agent.ResetPath();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.anim.SetBool("isAttacking", false);
        //Debug.Log("Has salido del estado de Attack Melee");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (_timer > _timeBetweenHits)
        {
            _timer = 0f;
        }

        _timer += Time.deltaTime;

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR > distanceToStopAttackStateSQR)
        {
            enemy.doAttack = false;
            enemy.doChase = true;
            enemy.enemyStateMachine.ChangeState(enemy.enemyStateMachine.EnemyRetreatState);
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
