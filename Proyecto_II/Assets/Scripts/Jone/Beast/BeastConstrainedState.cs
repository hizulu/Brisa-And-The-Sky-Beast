using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Jone Sainz Egea
// 16/04/2025
// Estado de restricción de la bestia, contiene un árbol de comportamiento
public class BeastConstrainedState : BeastState
{
    private Node behaviorTree;
    private Blackboard blackboard;
    private Transform playerTransform;

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Beast has entered Constrained State");

        blackboard = beast.blackboard;
        SetUpFlags();

        playerTransform = beast.playerTransform;

        behaviorTree = SetupConstrainedBehaviorTree(beast);
    }

    public override void OnUpdate(Beast beast)
    {
        behaviorTree?.Evaluate();
    }

    public override void OnExit(Beast beast)
    {
        Debug.Log("Beast is leaving Constrained State");

        blackboard.SetValue("menuOpenedFromOtherState", false);
        blackboard.SetValue("isConstrained", false);
    }

    private Node SetupConstrainedBehaviorTree(Beast beast)
    {
        return new Selector(new List<Node>
        {
            new CheckFlag(blackboard, "goToPlayer",
                new GoToPlayer(blackboard, beast, playerTransform, beast.arrivalThreshold)),
            new CheckFlag(blackboard, "reachedPlayer",
                new WaitForOrder(blackboard, beast, playerTransform, beast.interactionThreshold, 8f)),
            new CheckFlag(blackboard, "menuOpened",
                new Selector (new List<Node>
                {
                    new CheckFlag(blackboard, "isOptionPet",
                        new PetBeast(blackboard, beast)),
                    new CheckFlag(blackboard, "isOptionHeal",
                        new HealBeast(blackboard, beast)),
                    new CheckFlag(blackboard, "isOptionAttack",
                        new TransitionToBeastState(beast, new BeastCombatState())),
                    new CheckFlag(blackboard, "isOptionMount",
                        new TransitionToBeastState(beast, new BeastMountedState())),
                    new CheckFlag(blackboard, "isOptionAction",
                        new SpecificActions()),
                    new AlwaysTrue()
                })),
            new Sequence(new List<Node>
            {
                new IdleBehavior(blackboard, beast, 40f, 20f),
                new TransitionToBeastState(beast, new BeastFreeState())
            })
        });
    }

    private void SetUpFlags()
    {
        blackboard.SetValue("isConstrained", true);
        if (blackboard.TryGetValue("menuOpenedFromOtherState", out bool flag) && flag == true)
        {
            blackboard.SetValue("goToPlayer", false);
            blackboard.SetValue("reachedPlayer", false);
            blackboard.SetValue("menuOpened", true);
            Debug.Log("Flags set from other state");
        }
        else
        {           
            blackboard.SetValue("goToPlayer", true);
            blackboard.SetValue("reachedPlayer", false);
            blackboard.SetValue("menuOpened", false);
            Debug.Log("Flags set from called");
        }
        blackboard.SetValue("isCoroutineActive", false);
        blackboard.SetValue("isOptionPet", false);
        blackboard.SetValue("isOptionHeal", false);
        blackboard.SetValue("isOptionAttack", false);
        blackboard.SetValue("isOptionMount", false);
        blackboard.SetValue("isOptionAction", false);
    }
}
