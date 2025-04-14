using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckFlag : Node
{
    private Blackboard _blackboard;
    private string _key;
    private Node _child;
    private bool _expectedValue;

    public CheckFlag(Blackboard blackboard, string key, Node child, bool expectedValue = true)
    {
        _blackboard = blackboard;
        _key = key;
        _child = child;
        _expectedValue = expectedValue;
    }

    public override NodeState Evaluate()
    {
        if (_blackboard.TryGetValue(_key, out bool flag) && flag == _expectedValue)
        {
            return _child.Evaluate();
        }

        return NodeState.FAILURE;
    }
}
