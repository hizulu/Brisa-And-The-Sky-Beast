using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 16/04/2025
public class IdleBehavior : Node
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _probabilityToSit;
    private float _probabilityToSleep;

    public IdleBehavior(Blackboard blackboard, Beast beast, float probabilityToSit, float probabilityToSleep)
    {
        _blackboard = blackboard;
        _beast = beast;
        _probabilityToSit = probabilityToSit;
        _probabilityToSleep = probabilityToSleep;
    }

    public override NodeState Evaluate()
    {
        Node sequence = new Sequence(new List<Node>
        {
            new SetRandomFlag(_blackboard, "shouldSit", _probabilityToSit),
            new Selector(new List<Node>
            {
                new CheckFlag(_blackboard, "shouldSit",
                    new CheckFlag(_blackboard, "isCoroutineActive",
                        new Sit(_blackboard, _beast, 4f, 7f)), false),
                new AlwaysTrue()
            }),
            new SetRandomFlag(_blackboard, "shouldSleep", _probabilityToSleep),
            new Selector(new List<Node>
            {
                new CheckFlag(_blackboard, "shouldSleep",
                    new CheckFlag(_blackboard, "isCoroutineActive",
                        new Sleep(_blackboard, _beast, 6f, 12f)), false),
                new AlwaysTrue()
            }),
            new CheckFlag(_blackboard, "isCoroutineActive",
                new GoBackToLooking(_blackboard), false)
        });

        return sequence.Evaluate();
    }
}
