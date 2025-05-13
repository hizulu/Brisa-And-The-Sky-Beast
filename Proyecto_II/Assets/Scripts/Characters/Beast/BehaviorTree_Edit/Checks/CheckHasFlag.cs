using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 16/04/2025
// Nodo que comprueba que haya un valor guardado en la blackboard
// Su resultado depende de si se espera que lo tenga o no
// Por defecto, éxito si tiene el valor
public class CheckHasKey : Node
{
    private Blackboard _blackboard;
    private string _key;
    private Node _child;
    private bool _expectedValue;

    public CheckHasKey(Blackboard blackboard, string key, Node child, bool expectedValue = true)
    {
        _blackboard = blackboard;
        _key = key;
        _child = child;
        _expectedValue = expectedValue;
    }

    public override NodeState Evaluate()
    {
        if (_blackboard.HasKey(_key) && _expectedValue)
        {
            return _child.Evaluate();
        }

        return NodeState.FAILURE;
    }
}
