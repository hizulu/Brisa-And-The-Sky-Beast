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

    public override void OnEnter(Beast beast)
    {
        Debug.Log("Beast has entered Free State");

        // Activamos la flag en el Blackboard
        beast.blackboard.SetValue("isConstrained", false);
        beast.blackboard.SetValue("lookForTarget", true);

        // Creamos el árbol de comportamiento libre
        behaviorTree = SetupFreeBehaviorTree(beast);
    }

    public override void OnUpdate(Beast beast)
    {
        if (behaviorTree != null)
            behaviorTree.Evaluate();
    }

    public override void OnExit(Beast beast)
    {
        Debug.Log("Beast is leaving Free State");
    }

    private Node SetupFreeBehaviorTree(Beast beast)
    {
        Blackboard blackboard = beast.blackboard;

        // Subárbol: Comportamiento de punto de interés
        Node interestSubtree = new Sequence(new List<Node>
        {
            new GetInterestPoint(beast, beast.freeRoamRadius),
            new GoToInterestPoint(beast, beast.arrivalThreshold),
            new Selector(new List<Node>
            {
                new CheckSmellable(blackboard),
                new Smell(blackboard, beast, 0.5f, 5f),
                new AlwaysTrue() // Si no se puede oler, continuar
            }),
            new IdleBehavior(blackboard, beast)
        });

        // Subárbol: Comportamiento si no se encuentra objetivo
        Node idleFallback = new IdleBehavior(blackboard, beast);

        // Selector principal: decide si hace algo con un POI o si descansa
        return new Selector(new List<Node>
        {
            interestSubtree,
            idleFallback
        });
    }
}
