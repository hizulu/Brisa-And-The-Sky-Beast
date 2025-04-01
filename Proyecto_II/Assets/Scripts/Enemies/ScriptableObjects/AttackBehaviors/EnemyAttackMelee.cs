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
        distanceToStopAttackStateSQR = distanceToStopAttackState * distanceToStopAttackState;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.anim.SetBool("isAttacking", false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector3.zero);

        if (_timer > _timeBetweenHits)
        {
            _timer = 0f;
            enemy.anim.SetBool("isAttacking", true);
        }

        _timer += Time.deltaTime;
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();

        float distanceToPlayerSQR = (enemy.transform.position - playerTransform.position).sqrMagnitude;

        if (distanceToPlayerSQR > distanceToStopAttackStateSQR)
        {
            enemy.doAttack = false;
            enemy.doChase = true;
        }
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
