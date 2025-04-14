using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 14/04/2025
// Nodo que realiza la acción de estar sentado por un tiempo aleatorio
public class Sit : Node
{
    private BeastBehaviorTree _beastBehaviorTree;
    private Blackboard _blackboard;
    private float _minDuration;
    private float _maxDuration;

    private bool _isRunning = false;
    private bool _hasFinished = false;

    public Sit(Blackboard blackboard, BeastBehaviorTree beastBehaviorTree, float minDuration, float maxDuration)
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
                _beastBehaviorTree.StartNewCoroutine(Sitting(Random.Range(_minDuration, _maxDuration)));
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

    private IEnumerator Sitting(float duration)
    {
        BeastBehaviorTree.anim.SetBool("isWalking", false);
        BeastBehaviorTree.anim.SetBool("isSitting", true);

        Debug.Log("Iniciando animación de estar sentado");

        yield return new WaitForSeconds(duration);

        BeastBehaviorTree.anim.SetBool("isSitting", false);

        _blackboard.SetValue("lookForTarget", true);
        _blackboard.ClearKey("shouldSit");

        Debug.Log("Finished sitting");

        _hasFinished = true;
    }
}
