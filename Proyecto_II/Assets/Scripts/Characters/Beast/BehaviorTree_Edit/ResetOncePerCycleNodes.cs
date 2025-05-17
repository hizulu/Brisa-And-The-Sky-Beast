using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 17/05/2025
public class ResetOncePerCycleNodes : Node
{
    private Blackboard _blackboard;

    public ResetOncePerCycleNodes(Blackboard blackboard)
    {
        _blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        ResetFlags();
        if (_blackboard.TryGetValue("oncePerCycleNodes", out List<OncePerCycle> nodes))
        {
            foreach (var node in nodes)
            {
                // Debug.Log("one node reset");
                node.Reset();
            }

            nodes.Clear();
        }
        Debug.Log("Once per cycle nodes have been reset");

        state = NodeState.SUCCESS;
        return state;
    }

    private void ResetFlags()
    {
        _blackboard.ClearKey("shouldSit");
        _blackboard.ClearKey("shouldSleep");
        _blackboard.ClearKey("shouldStretch");
        _blackboard.ClearKey("shouldHowl");
        _blackboard.SetValue("isCoroutineActive", false);
    }
}
