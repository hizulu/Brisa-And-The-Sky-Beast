using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 28/04/2025
public class GetCombatTarget : Node
{
    private Blackboard _blackboard;
    private Beast _beast;

    private GameObject _target;

    public GetCombatTarget(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
    }

    public override NodeState Evaluate()
    {
        if (_blackboard.HasKey("targetForCombat"))
        {
            state = NodeState.SUCCESS;
            return state;
        }

        if (_beast.enemiesInRange.Count == 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _target = LookForCombatTarget();

        if (_target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _blackboard.SetValue("targetForCombat", _target);
        state = NodeState.SUCCESS;
        return state;
    }

    private GameObject LookForCombatTarget()
    {
        GameObject enemyTarget = null;
        float bestDistanceToEnemy = 0f;

        foreach (GameObject enemy in _beast.enemiesInRange)
        {
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(_beast.transform.position, enemy.transform.position);
                if (distanceToEnemy > bestDistanceToEnemy)
                {
                    bestDistanceToEnemy = distanceToEnemy;
                    enemyTarget = enemy;
                }
            }
        }

        return enemyTarget;
    }
}
