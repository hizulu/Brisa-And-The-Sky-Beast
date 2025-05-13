using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 14/04/2025
// Nodo que funciona para asegurar que la bestia busque un nuevo objetivo
public class GoBackToLooking : Node
{
    private Blackboard _blackboard;
    public GoBackToLooking(Blackboard blackboard)
    {
        _blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        _blackboard.SetValue("lookForTarget", true);
        return NodeState.SUCCESS;
    }
}
