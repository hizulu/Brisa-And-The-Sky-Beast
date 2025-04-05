using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 04/04/2025
namespace BehaviorTree
{
    // If one child is a success it is a success
    public class Selector : Node
    {
        public Selector() : base("Selector") { }
        public Selector(List<Node> children) : base("Selector", children) { }
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}
