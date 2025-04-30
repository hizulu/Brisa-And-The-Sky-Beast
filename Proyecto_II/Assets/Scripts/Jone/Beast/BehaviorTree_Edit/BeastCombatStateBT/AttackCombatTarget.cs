using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 29/04/2025
public class AttackCombatTarget : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public AttackCombatTarget(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
    }

    public override NodeState Evaluate()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;

            _beast.agent.ResetPath(); // TODO: si hay que reposicionar a la bestia se haría aquí

            _beast.anim.SetBool("isWalking", false);
            _beast.anim.SetBool("isAttackingSwipe", true);

            Debug.Log("Starting to attack");
            _beast.StartNewCoroutine(Attacking(1f), this);
        }

        if (_hasFinished)
        {
            _isRunning = false;
            state = NodeState.SUCCESS;
        }

        else
        {
            state = NodeState.RUNNING;
        }

        return state;
    }

    private IEnumerator Attacking(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        OnCoroutineEnd();
    }

    public void OnCoroutineEnd()
    {
        if (_hasFinished) return;

        _beast.anim.SetBool("isAttackingSwipe", false);

        _blackboard.SetValue("reachedCombatTarget", false);
        _blackboard.SetValue("isCoroutineActive", false);
        _blackboard.SetValue("attacked", true);
        _blackboard.SetValue("menuOpened", false);
        _blackboard.ClearKey("targetForCombat");

        Debug.Log("Finished attacking");

        _hasFinished = true;
    }
}
