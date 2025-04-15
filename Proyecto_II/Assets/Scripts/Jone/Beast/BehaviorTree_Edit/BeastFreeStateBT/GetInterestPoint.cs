using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 05/04/2025
// Nodo que busca el punto de mayor interés, falla si no encuentra ninguno
public class GetInterestPoint : Node
{
    private Blackboard _blackboard;
    private Beast _beast;
    private float _searchRadius;

    private List<PointOfInterest> _interestPoints;

    public GetInterestPoint(Beast beast, float searchRadius)
    {
        _beast = beast;
        _searchRadius = searchRadius;

        _blackboard = _beast.blackboard;
    }

    public override NodeState Evaluate()
    {
        // Si ya tiene un objetivo válido, no hace falta buscar otro
        PointOfInterest currentTarget = _blackboard.GetValue<PointOfInterest>("target");
        if (currentTarget != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        // Buscar nuevos puntos de interés
        _interestPoints = GetPointsOfInterest();
        PointOfInterest bestPoint = GetHighestInterestPoint(_interestPoints);

        if (bestPoint != null)
        {
            _blackboard.SetValue("target", bestPoint);
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }

    private List<PointOfInterest> GetPointsOfInterest()
    {
        List<PointOfInterest> foundInterestPoints = new List<PointOfInterest>();
        Collider[] colliders = Physics.OverlapSphere(_beast.transform.position, _searchRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("InterestObject"))
            {
                PointOfInterest poi = col.GetComponent<PointOfInterest>();
                if (poi != null)
                {
                    foundInterestPoints.Add(poi);
                }
            }
        }

        return foundInterestPoints;
    }

    private PointOfInterest GetHighestInterestPoint(List<PointOfInterest> points)
    {
        PointOfInterest bestPoint = null;
        float highestInterest = 0f;

        foreach (var point in points)
        {
            float interestValue = point.GetInterestValue(_beast.transform);
            if (interestValue > highestInterest)
            {
                highestInterest = interestValue;
                bestPoint = point;
            }
        }
        return bestPoint;
    }
}
