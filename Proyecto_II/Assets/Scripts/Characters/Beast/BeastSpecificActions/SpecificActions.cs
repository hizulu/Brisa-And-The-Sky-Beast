using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 17/04/2025
public class SpecificActions : Node
{
    private Blackboard _blackboard;
    private Beast _beast;

    private float _searchRadius = 5f;

    private bool _isActioned = false;

    public SpecificActions(Blackboard blackboard, Beast beast)
    {
        _blackboard = blackboard;
        _beast = beast;
    }

    public override NodeState Evaluate()
    {
        if(_isActioned)
            return NodeState.SUCCESS;

        BeastActionable targetZone = GetClosestActionableZone();
        if (targetZone != null)
        {
            Debug.Log("Ha encontrado target zone");
            if (targetZone.OnBeast())
            {
                _isActioned = true;
            }
        }

        _blackboard.SetValue("menuOpened", false);
        _blackboard.SetValue("isOptionAction", false);

        return NodeState.SUCCESS;
    }

    private BeastActionable GetClosestActionableZone()
    {
        Collider[] colliders = Physics.OverlapSphere(_beast.transform.position, _searchRadius);

        BeastActionable closestZone = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            BeastActionable zone = col.GetComponent<BeastActionable>();
            if (zone != null)
            {
                float distance = Vector3.Distance(_beast.transform.position, zone.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestZone = zone;
                }
            }
        }

        return closestZone;
    }
}
