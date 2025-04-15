using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class IdleBehavior : Node
{
    private Blackboard _blackboard;
    private Beast _beast;

    public IdleBehavior(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
    }

    public override NodeState Evaluate()
    {
        Node sequence = new Sequence(new List<Node>
        {
            new SetRandomFlag(_blackboard, "shouldSit", 50f),
            new Selector(new List<Node>
            {
                new CheckFlag(_blackboard, "shouldSit", new Sit(_blackboard, _beast, 3f, 6f)),
                new AlwaysTrue()
            }),
            new SetRandomFlag(_blackboard, "shouldSleep", 30f),
            new Selector(new List<Node>
            {
                new CheckFlag(_blackboard, "shouldSleep", new Sleep(_blackboard, _beast, 5f, 10f)),
                new AlwaysTrue()
            }),
            new GoBackToLooking(_blackboard)
        });

        return sequence.Evaluate();
    }
}
