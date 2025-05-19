using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 02/05/2025
public class Stretch : Node, ICoroutineNode
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _duration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public Stretch(Blackboard blackboard, Beast beast)
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

            _beast.agent.ResetPath();

            _beast.anim.SetBool("isWalking", false);
            _beast.anim.SetTrigger("stretch");
            Debug.Log("stretch triggered");

            _duration = AnimationDurationDatabase.Instance.GetClipDuration("Beast_Stretch");

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
        _blackboard.ClearKey("shouldStretch");

        _hasFinished = true;
    }
}
