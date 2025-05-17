using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
// Jone Sainz Egea
// 16/05/2025
public class OncePerCycle : Node
{
    private Blackboard _blackboard;
    private readonly Node _child;
    private bool _hasRunThisCycle = false;

    public OncePerCycle(Blackboard blackboard, Node child)
    {
        _blackboard = blackboard;
        _child = child;

        if (_blackboard != null)
        {
            if (!_blackboard.TryGetValue("oncePerCycleNodes", out List<OncePerCycle> list))
            {
                list = new List<OncePerCycle>();
                _blackboard.SetValue("oncePerCycleNodes", list);
            }
            list.Add(this);
        }
    }

    public override NodeState Evaluate()
    {
        if (_hasRunThisCycle)
        {
            state = NodeState.FAILURE;
            return state;
        }

        NodeState result = _child.Evaluate();

        if (result == NodeState.SUCCESS)
        {
            _hasRunThisCycle = true;
            state = NodeState.SUCCESS;
        }
        else if (result == NodeState.RUNNING)
        {
            state = NodeState.RUNNING;
        }
        else // FAILURE
        {
            state = NodeState.FAILURE;
        }

        return state;
    }

    public void Reset()
    {
        _hasRunThisCycle = false;
    }
}
