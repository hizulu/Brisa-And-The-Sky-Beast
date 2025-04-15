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
    private Blackboard _blackboard;
    private Beast _beast;
    private Transform _transform;
    private float _arrivalThreshold;

    private bool _wasWalking = false;

    public GoToInterestPoint(Beast beast, float arrivalThreshold)
    {
        _beast = beast;
        _arrivalThreshold = arrivalThreshold;

        _blackboard = _beast.blackboard;
        _transform = _beast.transform;
    }

    public override NodeState Evaluate()
    {
        PointOfInterest target = _blackboard.GetValue<PointOfInterest>("target");

        float distance = Vector3.Distance(_transform.position, target.transform.position);

        if (distance < _arrivalThreshold)
            return ArriveAtTarget(target);

        UpdateDestinationIfNeeded(target);

        if (!IsPathValid())
            return Failure($"Path to {target.name} is invalid or partial.");

        SetWalkingState(true);
        state = NodeState.RUNNING;

        return state;
    }

    private NodeState ArriveAtTarget(PointOfInterest target)
    {
        SetWalkingState(false);
        target.ConsumeInterest();
        _blackboard.SetValue("reachedTarget", true);
        _blackboard.ClearKey("target");

        Debug.Log($"Reached {target.name}, interest consumed.");
        state = NodeState.SUCCESS;
        return state;
    }

    private void UpdateDestinationIfNeeded(PointOfInterest target)
    {
        if (_beast.agent.destination != target.transform.position)
        {
            _beast.agent.SetDestination(target.transform.position);
        }
    }

    private bool IsPathValid()
    {
        var pathStatus = _beast.agent.pathStatus;
        return pathStatus == NavMeshPathStatus.PathComplete;
    }

    private void SetWalkingState(bool walking)
    {
        if (_wasWalking != walking)
        {
            _beast.anim.SetBool("isWalking", walking);
            _wasWalking = walking;
        }
    }

    private NodeState Failure(string reason)
    {
        Debug.LogWarning(reason);
        state = NodeState.FAILURE;
        return state;
    }
}
