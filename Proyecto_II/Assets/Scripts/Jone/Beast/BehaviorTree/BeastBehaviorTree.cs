using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastBehaviorTree : BTree
{
    [SerializeField] private float searchRadius = 30f;
    
    public BeastBehaviorTree()
    {
    }

    protected override Node SetupTree()
    {
        Node root = new GetInterestPoint(transform, searchRadius);

        return root;
    }
}
