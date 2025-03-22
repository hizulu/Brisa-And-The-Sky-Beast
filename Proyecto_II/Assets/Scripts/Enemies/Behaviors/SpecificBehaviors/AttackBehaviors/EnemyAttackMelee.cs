using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Melee", menuName = "Enemy Logic/Attack Logic/Melee")]
public class EnemyAttackMelee : EnemyAttackSOBase
{
    //[SerializeField] private float _timeBetweenHits = 2f;
    //[SerializeField] private float _attackDamage = 20f;

    //private float _timer;

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

        //enemy.MoveEnemy(Vector2.zero);

        //if (_timer > _timeBetweenHits)
        //{
        //    _timer = 0f;
        //    enemy.anim.SetTrigger("ataca");
        //    enemy.PlaySonidosEnem(1); // Sonido ataque enemigo
        //    enemy.player.GetComponent<Player>().recibeDamage(_attackDamage);

        //}

        //if (!enemy.IsWithinStrikingDistance)
        //{
        //    enemy.StateMachine.ChangeState(enemy.ChaseState);
        //}

        //_timer += Time.deltaTime;
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
