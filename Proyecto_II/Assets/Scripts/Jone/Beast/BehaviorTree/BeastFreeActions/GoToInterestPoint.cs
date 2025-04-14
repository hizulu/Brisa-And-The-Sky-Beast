using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// 06/04/2025
// Nodo que va al punto de mayor interés, éxito si llega a él
public class GoToInterestPoint : Node
{
    private Transform _transform;
    private float _arrivalThreshold;
    private NavMeshAgent _agent;
    private Blackboard _blackboard;
    private bool _wasWalking = false;

    public GoToInterestPoint(Transform transform, NavMeshAgent agent, float arrivalThreshold, Blackboard blackboard) : base()
    {
        _transform = transform;
        _agent = agent;
        _arrivalThreshold = arrivalThreshold;
        _blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        PointOfInterest target = _blackboard.GetValue<PointOfInterest>("target");

        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float distance = Vector3.Distance(_transform.position, target.transform.position);

        if (distance < _arrivalThreshold)
        {
            if (_wasWalking)
            {
                BeastBehaviorTree.anim.SetBool("isWalking", false);
                target.ConsumeInterest();
                _blackboard.SetValue("hasArrived", true);

                Debug.Log($"Reached {target.name}, interest consumed.");
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

        if (_agent.destination != target.transform.position)
            _agent.SetDestination(target.transform.position);

        // Verificar si el destino es alcanzable
        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid || _agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning($"Path to {target.name} is invalid or partial.");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
