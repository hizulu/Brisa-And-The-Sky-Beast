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
        beast.anim.SetBool("isAttackingSwipe", false);

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

            //new CheckFlag(blackboard, "goToPlayer",
            //    new GoToPlayer(blackboard, beast, playerTransform, beast.arrivalThreshold)),
            //new CheckFlag(blackboard, "reachedPlayer",
            //    new WaitForOrder(blackboard, beast, playerTransform, beast.interactionThreshold, 8f)),
            //new CheckFlag(blackboard, "menuOpened",
            //    new Selector (new List<Node>
            //    {
            //        new CheckFlag(blackboard, "isOptionPet",
            //            new PetBeast(blackboard, beast)),
            //        new CheckFlag(blackboard, "isOptionHeal",
            //            new HealBeast(blackboard, beast, beast.healingAmount)),
            //        new CheckFlag(blackboard, "isOptionAttack",
            //            new TransitionToBeastState(beast, new BeastCombatState())),
            //        new CheckFlag(blackboard, "isOptionMount",
            //            new TransitionToBeastState(beast, new BeastMountedState())),
            //        new CheckFlag(blackboard, "isOptionAction",
            //            new SpecificActions(blackboard, beast)),
            //        new AlwaysTrue()
            //    })),
            //new Sequence(new List<Node>
            //{
            //    new IdleBehavior(blackboard, beast, 40f, 20f),
            //    new TransitionToBeastState(beast, new BeastFreeState())
            //})
        });
    }
}
