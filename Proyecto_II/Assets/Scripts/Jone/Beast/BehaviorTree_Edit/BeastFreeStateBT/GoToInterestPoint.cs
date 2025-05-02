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

    private Transform _target;
    private PointOfInterest _pointOfInterest;
    private bool _targetIsBrisa = false;
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
        if(_blackboard.TryGetValue("target", out PointOfInterest targetPoint))
        {
            _targetIsBrisa = false;
            _target = targetPoint.transform;
            _pointOfInterest = targetPoint;
        }
        else if(_blackboard.TryGetValue("target", out Transform targetBrisa))
        {
            _targetIsBrisa = true;
            _target = targetBrisa;
        }

        float distance = Vector3.Distance(_transform.position, _target.transform.position);

        if (distance < _arrivalThreshold + 2)
            return ArriveAtTarget();

        UpdateDestinationIfNeeded(_target);

        if (!IsPathValid())
            return Failure($"Path to {_target.name} is invalid or partial.");

        SetWalkingState(true);
        state = NodeState.RUNNING;

        return state;
    }

    private NodeState ArriveAtTarget()
    {
        SetWalkingState(false);
        if (_targetIsBrisa)
        {
            // TODO: ConsumeInterestInBrisa
            Debug.Log("Consume interés en Brisa");
        }
        else
        {
            Debug.Log("Consume interés en objeto");
            _pointOfInterest.ConsumeInterest();
        }
        _blackboard.SetValue("reachedTarget", true);
        _blackboard.ClearKey("target");
        _beast.agent.ResetPath();

        Debug.Log($"Reached {_target.name}, interest consumed.");
        state = NodeState.SUCCESS;
        return state;
    }

    private void UpdateDestinationIfNeeded(Transform target)
    {
        if (_beast.agent.destination != target.position)
        {
            _beast.agent.SetDestination(target.position);
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
