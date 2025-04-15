using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

// Jone Sainz Egea
// 15/04/2025
// Nodo que se encarga de que la bestia vaya a la posici�n del jugador cuando se le llama
public class GoToPlayer : Node
{
    private Blackboard _blackboard;
    private Beast _beast;
    private Transform _playerTransform;
    private float _arrivalThreshold;

    private bool _wasWalking = false;

    public GoToPlayer(Blackboard blackboard, Beast beast, Transform playerTransform, float arrivalThreshold)
    {
        _blackboard = blackboard;
        _playerTransform = playerTransform;
        _arrivalThreshold = arrivalThreshold;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(_beast.transform.position, _playerTransform.position);

        if (distance < _arrivalThreshold)
        {
            if (_wasWalking)
            {
                _beast.anim.SetBool("isWalking", false);
                // Cambiar�a a wait for order
                _blackboard.SetValue("hasArrived", true);

                Debug.Log("Reached player.");
                _wasWalking = false;
            }
            state = NodeState.SUCCESS;
            return state;
        }

        if (!_wasWalking)
        {
            _beast.anim.SetBool("isWalking", true);
            _wasWalking = true;
        }

        if (_beast.agent.destination != _playerTransform.position) //&& !_beast.beastWaitingOrder)
            _beast.agent.SetDestination(_playerTransform.position);

        // Verificar si el destino es alcanzable
        if (_beast.agent.pathStatus == NavMeshPathStatus.PathInvalid || _beast.agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Path to player is invalid or partial.");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
