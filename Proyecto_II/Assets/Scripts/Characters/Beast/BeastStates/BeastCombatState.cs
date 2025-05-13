using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 19/04/2025
public class BeastCombatState : BeastState
{
    private Node behaviorTree;
    private Blackboard blackboard;

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Beast has entered combat state");

        blackboard = beast.blackboard;

        blackboard.ClearKey("targetForCombat");
        blackboard.SetValue("reachedCombatTarget", false);
        blackboard.SetValue("attacked", false);

        behaviorTree = SetupCombatBehaviorTree(beast);
    }
    public override void OnUpdate(Beast beast)
    {
        if (behaviorTree != null)
            behaviorTree.Evaluate();
    }
    public override void OnExit(Beast beast)
    {
        blackboard.ClearKey("targetForCombat");
        blackboard.SetValue("reachedCombatTarget", false);
        blackboard.SetValue("attacked", false);

        Debug.Log("Beast has leaved combat state");
    }

    private Node SetupCombatBehaviorTree(Beast beast)
    {
        return new Sequence(new List<Node>
        {
            new GetCombatTarget(blackboard, beast),
            new GoToCombatTarget(blackboard, beast, beast.arrivalThreshold),
            new CheckFlag(blackboard, "reachedCombatTarget",
                new AttackCombatTarget(blackboard, beast)),
            new CheckFlag(blackboard, "attacked",
                new CooldownForCombat(blackboard, beast, 3f))
        });
    }
}
