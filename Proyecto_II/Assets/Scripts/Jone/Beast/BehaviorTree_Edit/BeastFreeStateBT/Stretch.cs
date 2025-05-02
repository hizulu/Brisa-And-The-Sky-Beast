using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretch : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _duration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public Stretch(Blackboard blackboard, Beast beast, float duration)
    {
        _blackboard = blackboard;
        _beast = beast;
        _duration = duration;
    }

    public override NodeState Evaluate()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;

            _beast.agent.ResetPath();

            _beast.anim.SetBool("isWalking", false);
            _beast.anim.SetTrigger("stretch");

            _beast.StartNewCoroutine(Stretching(_duration), this);
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

    private IEnumerator Stretching(float duration)
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

        Debug.Log("Doing Idle");

        _hasFinished = true;
    }
}
