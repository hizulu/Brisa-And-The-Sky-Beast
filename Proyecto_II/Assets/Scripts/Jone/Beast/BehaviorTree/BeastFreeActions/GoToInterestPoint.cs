using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToInterestPoint : Node
{
    private Transform _transform;
    private NavMeshAgent _agent;
    private PointOfInterest _target;

    public GoToInterestPoint(Transform transform, NavMeshAgent agent) : base()
    {
        _transform = transform;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        _target = (PointOfInterest)GetData("target");
        if (_target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            _agent.SetDestination(_target.transform.position);
            if(Vector3.Distance(_transform.position, _target.transform.position) < 6f)
            {
                state = NodeState.SUCCESS;
            }
        }
        state = NodeState.RUNNING;
        return state;
    }
}
