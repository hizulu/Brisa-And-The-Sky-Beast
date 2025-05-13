using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class OncePerCycle : Node
{
    private readonly Node _child;
    private bool _hasRun;

    public OncePerCycle(Node child)
    {
        _child = child;
        _hasRun = false;
    }

    public override NodeState Evaluate()
    {
        if (_hasRun)
        {
            return NodeState.SUCCESS;
        }

        var result = _child.Evaluate();

        if (result == NodeState.SUCCESS || result == NodeState.FAILURE)
        {
            _hasRun = true;
        }

        return result;
    }

    public void Reset()
    {
        _hasRun = false;
    }
}
