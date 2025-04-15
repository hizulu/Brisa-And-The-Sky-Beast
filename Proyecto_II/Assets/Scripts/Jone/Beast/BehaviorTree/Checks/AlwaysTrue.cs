using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 15/04/2025
// Nodo que sirve para debugging del árbol (ver en qué punto falla)
public class AlwaysTrue : Node
{
    public AlwaysTrue() { }

    public override NodeState Evaluate()
    {
        Debug.Log("Always true");
        return NodeState.SUCCESS;
    }
}
