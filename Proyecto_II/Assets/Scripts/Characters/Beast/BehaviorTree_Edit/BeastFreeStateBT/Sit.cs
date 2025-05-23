using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 14/04/2025
// Nodo que realiza la acción de estar sentado por un tiempo aleatorio
public class Sit : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public Sit(Blackboard blackboard, Beast beast, float minDuration, float maxDuration)
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
            _beast.anim.SetBool("isSitting", true);
            _beast.SfxBeast.PlayRandomSFX(BeastSFXType.Idle);

            _beast.StartNewCoroutine(Sitting(Random.Range(_minDuration, _maxDuration)), this);
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

        _beast.anim.SetBool("isSitting", false);

        _blackboard.SetValue("lookForTarget", true);
        _blackboard.ClearKey("shouldSit");

        Debug.Log("Finished sitting");

        _hasFinished = true;
    }
}
