using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
// Jone Sainz Egea
// 15/04/2025
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
            // Debug.Log($"Flag {_key} es: {flag}");
            return _child.Evaluate();
        } else if (!_blackboard.HasKey(_key))
            Debug.LogWarning($"Looking for a flag that doesn't exist: {_key}");

        return NodeState.FAILURE;
    }
}
