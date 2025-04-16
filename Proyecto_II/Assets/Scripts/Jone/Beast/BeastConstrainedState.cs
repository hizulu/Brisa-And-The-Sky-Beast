using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        blackboard.SetValue("isConstrained", true);
        blackboard.SetValue("goToPlayer", true);
        blackboard.SetValue("reachedPlayer", false);
        blackboard.SetValue("isCoroutineActive", false);

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

        blackboard.SetValue("isConstrained", false);
    }

    private Node SetupConstrainedBehaviorTree(Beast beast)
    {
        return new Selector(new List<Node>
        {
            new CheckFlag(blackboard, "goToPlayer",
                new GoToPlayer(blackboard, beast, playerTransform, beast.arrivalThreshold), false),
            new CheckFlag(blackboard, "reachedPlayer",
                new WaitForOrder(blackboard, beast, playerTransform, beast.interactionThreshold, 8f)),
            new AlwaysTrue()
            //new Selector(new List<Node>
            //{
            //    // Si jugador se aleja mientras está sentado
            //    new Sequence(new List<Node>
            //    {
            //        new CheckFlag(blackboard, "playerIsNear", new AlwaysTrue(), false), // jugador se alejó
            //        new Wait(beast, 2f), // espera 2 seg
            //        new IdleBehavior(blackboard, beast, 40f, 20f),
            //        new ExitToFreeState(beast)
            //    }),

            //    // Si se abre el menú de interacción
            //    new Sequence(new List<Node>
            //    {
            //        new CheckFlag(blackboard, "interactionMenuOpen", new WaitUntilClosed(blackboard)), // nodo que espera a que el menú se cierre
            //        new Selector(new List<Node>
            //        {
            //            new CheckFlag(blackboard, "interactionSelected",
            //                new Sequence(new List<Node>
            //                {
            //                    new PerformInteraction(blackboard, beast), // ejecuta según lo elegido
            //                    new IdleBehavior(blackboard, beast, 40f, 20f),
            //                    new ExitToFreeState(beast)
            //                })),
            //            new Sequence(new List<Node>
            //            {
            //                new IdleBehavior(blackboard, beast, 40f, 20f),
            //                new ExitToFreeState(beast)
            //            })
            //        })
            //    }),

            //    // Si jugador se mantiene cerca y no hay menú
            //    new Sequence(new List<Node>
            //    {
            //        new Wait(beast, 8f),
            //        new IdleBehavior(blackboard, beast, 40f, 20f),
            //        new ExitToFreeState(beast)
            //    })
            //})
        });
    }
}
