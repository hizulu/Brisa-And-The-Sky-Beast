using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 02/05/2025
public class DoIdle : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public DoIdle(Blackboard blackboard, Beast beast, float minDuration, float maxDuration)
    {
        _blackboard = blackboard;
        _beast = beast;
        _minDuration = minDuration;
        _maxDuration = maxDuration;
    }

    public override NodeState Evaluate()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            _hasFinished = false;

            _beast.agent.ResetPath();

            _beast.anim.SetBool("isWalking", false);

            _beast.StartNewCoroutine(Sitting(Random.Range(_minDuration, _maxDuration)), this);
            _beast.SfxBeast.PlayRandomSFX(BeastSFXType.Idle);
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

    private IEnumerator Sitting(float duration)
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
