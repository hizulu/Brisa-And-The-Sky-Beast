using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 28/04/2025
public class CooldownForCombat : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _cooldownDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public CooldownForCombat(Blackboard blackboard, Beast beast, float cooldownDuration)
    {
        _blackboard = blackboard;
        _beast = beast;
        _cooldownDuration = cooldownDuration;
    }

    public override NodeState Evaluate()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;

            _beast.agent.ResetPath(); // TODO: si hay que reposicionar a la bestia se haría aquí

            _beast.anim.SetBool("isWalking", false);
            _beast.anim.SetTrigger("attackSwipe");

            Debug.Log("Cooling down");
            _beast.StartNewCoroutine(Cooldown(_cooldownDuration), this);
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

    private IEnumerator Cooldown(float duration)
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

        _blackboard.SetValue("isCoroutineActive", false);
        _blackboard.SetValue("attacked", false);

        Debug.Log("Cooled down");

        _hasFinished = true;
    }
}
