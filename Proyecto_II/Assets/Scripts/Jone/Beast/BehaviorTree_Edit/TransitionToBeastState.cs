using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 17/04/2025
public class TransitionToBeastState : Node
{
    private Beast _beast;
    private BeastState _newBeastState;

    public TransitionToBeastState(Beast beast, BeastState newState)
    {
        _beast = beast;
        _newBeastState = newState;
    }

    public override NodeState Evaluate()
    {
        _beast.TransitionToState(_newBeastState);
        return NodeState.SUCCESS;
    }
}
