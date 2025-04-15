using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 14/04/2025
// Nodo que realiza la acción de dormir por un tiempo aleatorio
public class Sleep : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public Sleep(Blackboard blackboard, Beast beast, float minDuration, float maxDuration)
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
            _beast.anim.SetBool("isSleeping", true);

            _beast.StartNewCoroutine(Sleeping(Random.Range(_minDuration, _maxDuration)), this);
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


    private IEnumerator Sleeping(float duration)
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

        _beast.anim.SetBool("isSleeping", false);

        _blackboard.SetValue("lookForTarget", true);
        _blackboard.ClearKey("shouldSleep");

        Debug.Log("Finished sleeping");

        _hasFinished = true;     
    }
}
