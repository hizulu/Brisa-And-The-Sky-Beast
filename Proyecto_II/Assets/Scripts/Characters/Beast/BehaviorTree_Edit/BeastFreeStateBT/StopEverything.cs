using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 16/05/2025
public class StopEverything : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public StopEverything(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
    }

    public override NodeState Evaluate()
    {
        if (_blackboard.GetValue<bool>("goingToPlayer"))
        {
            Debug.Log("Already going to player");
            return NodeState.SUCCESS;
        }
        
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;

            _beast.agent.ResetPath();

            Debug.Log("Stopping everything");
            _beast.StartNewCoroutine(StopCoroutines(0.12f), this);
        }

        if (_hasFinished)
        {
            _isRunning = false;
            state = NodeState.SUCCESS;
        }

        else
            state = NodeState.RUNNING;

        return state;
    }
    private IEnumerator StopCoroutines(float duration)
    {
        StopAnimations();
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
        _blackboard.SetValue("goingToPlayer", true);

        Debug.Log("Stopped everything");

        _hasFinished = true;
    }

    private void StopAnimations()
    {
        _beast.anim.SetBool("isWalking", false);
        _beast.anim.SetBool("isSitting", false);
        _beast.anim.SetBool("isSleeping", false);
        _beast.anim.SetBool("isRunning", false);
    }
}
