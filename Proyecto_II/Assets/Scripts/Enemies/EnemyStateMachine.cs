using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }
    public EnemyIdle EnemyIdleState { get; }
    public EnemyPatrol EnemyPatrolState { get; }
    public EnemyChase EnemyChaseState { get; }
    public EnemyAttack EnemyAttackState { get; }
    public EnemyRetreat EnemyRetreatState { get; }

    public EnemyStateMachine(Enemy _enemy)
    {
        Enemy = _enemy;

        EnemyIdleState = new EnemyIdle(this);
        EnemyPatrolState = new EnemyPatrol(this);
        EnemyChaseState = new EnemyChase(this);
        EnemyAttackState = new EnemyAttack(this);
        EnemyRetreatState = new EnemyRetreat(this);
    }
}
