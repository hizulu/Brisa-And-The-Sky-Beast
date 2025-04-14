using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class SetRandomFlag : Node
{
    private Blackboard _blackboard;
    private string _flagName;
    private float _probability; // Entre 0 y 100

    public SetRandomFlag(Blackboard blackboard, string flagName, float probability)
    {
        _blackboard = blackboard;
        _flagName = flagName;
        _probability = probability;
    }

    public override NodeState Evaluate()
    {
        if (!_blackboard.TryGetValue(_flagName, out bool _))
        {
            bool result = Random.Range(0f, 100f) < _probability;
            _blackboard.SetValue(_flagName, result);
        }

        return NodeState.SUCCESS;
    }
}
