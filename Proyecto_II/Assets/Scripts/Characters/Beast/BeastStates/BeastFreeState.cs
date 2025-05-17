using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 15/04/2025
// Estado de libertad de la bestia, contiene un árbol de comportamiento
public class BeastFreeState : BeastState
{
    private Node behaviorTree;
    private Blackboard blackboard;

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Beast has entered Free State");

        blackboard = beast.blackboard;

        // Activamos las flag en el Blackboard
        blackboard.SetValue("isConstrained", false);
        blackboard.SetValue("goingToPlayer", false);
        blackboard.SetValue("lookForTarget", true);
        blackboard.SetValue("reachedTarget", false);
        blackboard.SetValue("isCoroutineActive", false);

        // Creamos el árbol de comportamiento libre
        behaviorTree = SetupFreeBehaviorTree(beast);
    }

    public override void OnUpdate(Beast beast)
    {
        //Debug.Log($"[Tree] lookForTarget: {beast.blackboard.GetValue<bool>("lookForTarget")}, " +
        //      $"target: {beast.blackboard.HasKey("target")}, " +
        //      $"reachedTarget: {beast.blackboard.GetValue<bool>("reachedTarget")}, " +
        //      $"isCoroutineActive: {beast.blackboard.GetValue<bool>("isCoroutineActive")}");

        if (behaviorTree != null)
            behaviorTree.Evaluate();
    }

    public override void OnExit(Beast beast)
    {
        blackboard.SetValue("lookForTarget", true);
        blackboard.ClearKey("target");
        blackboard.SetValue("reachedTarget", false);

        Debug.Log("Beast has leaved Free State");
    }

    private Node SetupFreeBehaviorTree(Beast beast)
    {
        // Comportamiento del sistema de puntos de interés
        Node interestSubtree = new Selector(new List<Node>
        {
            new CheckFlag(blackboard, "lookForTarget",
                new GetInterestPoint(beast, beast.playerTransform, beast.freeRoamRadius)),
            new CheckHasKey(blackboard, "target",
                new GoToInterestPoint(beast, beast.arrivalThreshold)),
            new CheckFlag(blackboard, "reachedTarget",
                new Sequence(new List<Node>
                {
                    new CheckFlag(blackboard, "isCoroutineActive",
                        new Smell(blackboard, beast, 1f, 6f), false),
                    new IdleBehavior(blackboard, beast)
                })),
            new IdleBehavior(blackboard, beast), // Cuando no encuentra ningún objetivo
        });

        Node beastFreeTree = new Selector(new List<Node>
        {
            new CheckFlag(blackboard, "isConstrained",
                new TransitionToBeastState(beast, new BeastConstrainedState())),
            new CheckPlayerTooFar(beast, beast.playerTransform, 50f,
                new Sequence(new List<Node>
                {
                    new StopEverything(blackboard, beast),
                    new CheckFlag(blackboard, "goingToPlayer",
                        new GoToPlayerFree(blackboard, beast, beast.playerTransform, beast.arrivalThreshold)),
                })),
            new CheckFlag(blackboard, "isCoroutineActive", interestSubtree, false),
            new AlwaysTrue()
        });

        return beastFreeTree;
    }
}
