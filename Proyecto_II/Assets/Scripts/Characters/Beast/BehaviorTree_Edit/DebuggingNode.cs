using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class DebuggingNode : Node
{
    private string _name;
    public DebuggingNode(string name)
    {
        _name = name;
    }

    public override NodeState Evaluate()
    {
        Debug.Log($"Ha llegado al nodo de {_name}");
        return NodeState.SUCCESS;
    }
}
