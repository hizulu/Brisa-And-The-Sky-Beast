using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class GoToPlayer : Node
{
    private Blackboard _blackboard;
    private Transform _transform;
    private Transform _playerTransform;
    private NavMeshAgent _agent;
    private float _arrivalThreshold;

    private bool _wasWalking = false;

    public GoToPlayer(Blackboard blackboard, Transform transform, Transform playerTransform, NavMeshAgent agent, float arrivalThreshold)
    {
        _blackboard = blackboard;
        _transform = transform;
        _playerTransform = playerTransform;
        _agent = agent;
        _arrivalThreshold = arrivalThreshold;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(_transform.position, _playerTransform.position);

        if (distance < _arrivalThreshold)
        {
            if (_wasWalking)
            {
                BeastBehaviorTree.anim.SetBool("isWalking", false);
                // Cambiaría a wait for order
                _blackboard.SetValue("hasArrived", true);

                Debug.Log("Reached player.");
                _wasWalking = false;
            }
            state = NodeState.SUCCESS;
            return state;
        }

        if (!_wasWalking)
        {
            BeastBehaviorTree.anim.SetBool("isWalking", true);
            _wasWalking = true;
        }

        if (_agent.destination != _playerTransform.position && !BeastBehaviorTree.beastWaitingOrder)
            _agent.SetDestination(_playerTransform.position);

        // Verificar si el destino es alcanzable
        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid || _agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Path to player is invalid or partial.");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
