using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 14/04/2025
// Nodo que realiza la acción de dormir por un tiempo aleatorio
public class Sleep : Node
{
    private BeastBehaviorTree _beastBehaviorTree;
    private Blackboard _blackboard;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public Sleep(Blackboard blackboard, BeastBehaviorTree beastBehaviorTree, float minDuration, float maxDuration)
    {
        _blackboard = blackboard;
        _beastBehaviorTree = beastBehaviorTree;
        _minDuration = minDuration;
        _maxDuration = maxDuration;
    }

    public override NodeState Evaluate()
    {
        if (!BeastBehaviorTree.isConstrained)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _hasFinished = false;
                _beastBehaviorTree.StartNewCoroutine(Sleeping(Random.Range(_minDuration, _maxDuration)));
            }

            if (_hasFinished)
            {
                _isRunning = false;
                state = NodeState.SUCCESS;
            }

            else
                state = NodeState.RUNNING;
        }
        else
        {
            _isRunning = false;
            state = NodeState.FAILURE;
        }

        return state;
    }

    private IEnumerator Sleeping(float duration)
    {
        BeastBehaviorTree.anim.SetBool("isWalking", false);
        BeastBehaviorTree.anim.SetBool("isSleeping", true);

        Debug.Log("Iniciando animación de dormir");

        yield return new WaitForSeconds(duration);

        BeastBehaviorTree.anim.SetBool("isSleeping", false);

        _blackboard.SetValue("lookForTarget", true);
        _blackboard.ClearKey("shouldSleep");

        Debug.Log("Finished sleeping");

        _hasFinished = true;
    }
}
