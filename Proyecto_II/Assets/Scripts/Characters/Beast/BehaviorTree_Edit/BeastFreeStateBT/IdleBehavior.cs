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

    public IdleBehavior(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
    }

    public override NodeState Evaluate()
    {
        Node selector = new Selector(new List<Node>
        {
            new OncePerCycle(_blackboard,
                new Sequence(new List<Node>
                {
                    new DebuggingNode("secuencia sit"),
                    new SetRandomFlag(_blackboard, "shouldSit", 40f),
                    new CheckFlag(_blackboard, "shouldSit",
                        new CheckFlag(_blackboard, "isCoroutineActive",
                            new Sit(_blackboard, _beast, 4f, 7f), false)),
                    new DebuggingNode("termina sit")
                })),
            new OncePerCycle(_blackboard,
                new Sequence(new List<Node>
                {
                    new DebuggingNode("secuencia sleep"),
                    new SetRandomFlag(_blackboard, "shouldSleep", 30f),
                    new CheckFlag(_blackboard, "shouldSleep",
                        new CheckFlag(_blackboard, "isCoroutineActive",
                            new Sleep(_blackboard, _beast, 6f, 12f), false)),
                    new DebuggingNode("termina sleep")
                })),
            new Sequence(new List<Node>
            {
                new DoIdle(_blackboard, _beast, 3f, 5f),
                new Selector(new List<Node>
                {
                    new OncePerCycle(_blackboard,
                        new Sequence(new List<Node>
                        {
                            new DebuggingNode("secuencia stretch"),
                            new SetRandomFlag(_blackboard, "shouldStretch", 80f),
                            new CheckFlag(_blackboard, "shouldStretch",
                                new CheckFlag(_blackboard, "isCoroutineActive",
                                    new Stretch(_blackboard, _beast), false)),
                            new DebuggingNode("termina stretch")
                        })),
                    new OncePerCycle(_blackboard,
                        new Sequence(new List<Node>
                        {
                            new DebuggingNode("secuencia howl"),
                            new SetRandomFlag(_blackboard, "shouldHowl", 50f),
                            new CheckFlag(_blackboard, "shouldHowl",
                                new CheckFlag(_blackboard, "isCoroutineActive",
                                    new Howl(_blackboard, _beast), false)),
                            new DebuggingNode("termina howl")
                        })),
                    new AlwaysTrue()
                }),
                new ResetOncePerCycleNodes(_blackboard),
                new GoBackToLooking(_blackboard)
            })
        });

        Node sequence = new Sequence(new List<Node>
        {
            selector,
            new ResetOncePerCycleNodes(_blackboard),
            new GoBackToLooking(_blackboard)
        });

        return sequence.Evaluate();
    }
}
