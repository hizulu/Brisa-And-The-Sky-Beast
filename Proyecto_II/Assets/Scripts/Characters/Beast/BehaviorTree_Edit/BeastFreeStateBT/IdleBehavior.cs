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
        //Node sequence = new Sequence(new List<Node>
        //{
        //    new SetRandomFlag(_blackboard, "shouldSit", _probabilityToSit),
        //    new Selector(new List<Node> // Es un selector para que siempre devuelva true y siga al siguiente paso aunque no se realice la acción
        //    {
        //        new CheckFlag(_blackboard, "shouldSit",
        //            new CheckFlag(_blackboard, "isCoroutineActive",
        //                new Sit(_blackboard, _beast, 4f, 7f)), false),
        //        new AlwaysTrue()
        //    }),
        //    //TODO: creo que sit y sleep se pisan
        //    new SetRandomFlag(_blackboard, "shouldSleep", _probabilityToSleep),
        //    new Selector(new List<Node>
        //    {
        //        new CheckFlag(_blackboard, "shouldSleep",
        //            new CheckFlag(_blackboard, "isCoroutineActive",
        //                new Sleep(_blackboard, _beast, 6f, 12f)), false),
        //        new AlwaysTrue()
        //    }),
        //    new CheckFlag(_blackboard, "isCoroutineActive",
        //        new GoBackToLooking(_blackboard), false)
        //});

        Node selector = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new SetRandomFlag(_blackboard, "shouldSit", 40f),
                new CheckFlag(_blackboard, "shouldSit",
                    new CheckFlag(_blackboard, "isCoroutineActive",
                        new Sit(_blackboard, _beast, 4f, 7f)), false),
            }),
            new Sequence(new List<Node>
            {
                new SetRandomFlag(_blackboard, "shouldSleep", 30f),
                new CheckFlag(_blackboard, "shouldSleep",
                    new CheckFlag(_blackboard, "isCoroutineActive",
                        new Sleep(_blackboard, _beast, 6f, 12f)), false),
            }),
            new Sequence(new List<Node>
            {
                new DoIdle(_blackboard, _beast, 3f, 5f),
                new Selector(new List<Node>
                {
                    new OncePerCycle(
                        new Sequence(new List<Node>
                        {
                            new SetRandomFlag(_blackboard, "shouldStretch", 10f),
                            new CheckFlag(_blackboard, "shouldStretch",
                                new CheckFlag(_blackboard, "isCoroutineActive",
                                    new Stretch(_blackboard, _beast)), false),
                        })),
                    new OncePerCycle(
                        new Sequence(new List<Node>
                        {
                            new SetRandomFlag(_blackboard, "shouldHowl", 10f),
                            new CheckFlag(_blackboard, "shouldHowl",
                                new CheckFlag(_blackboard, "isCoroutineActive",
                                    new Howl(_blackboard, _beast)), false),
                        })),
                    new AlwaysTrue()
                }),
                new GoBackToLooking(_blackboard)
            })
        });

        return selector.Evaluate();
    }
}
