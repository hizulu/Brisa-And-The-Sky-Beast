using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

// Jone Sainz Egea
// 05/04/2025
// Nodo que busca el punto de mayor interés, falla si no encuentra ninguno
public class GetInterestPoint : Node
{
    private Transform _transform;
    private float _searchRadius;
    private Blackboard _blackboard;
    private List<PointOfInterest> _interestPoints;

    public GetInterestPoint(Transform transform, float searchRadius, Blackboard blackboard)
    {
        _transform = transform;
        _searchRadius = searchRadius;
        _blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        PointOfInterest currentTarget = _blackboard.GetValue<PointOfInterest>("target");
        if (currentTarget == null)
        {
            GetPointsOfInterest();
            PointOfInterest bestPoint = GetHighestInterestPoint(_interestPoints);
            if (bestPoint != null)
            {
                _blackboard.SetValue("target", bestPoint);
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.FAILURE;
                Debug.Log("No se ha encontrado ningún punto de interés.");
            }
        }
        else
        {
            Debug.Log("Ya tiene un objetivo, saliendo de GetInterestPoint sin recalcular...");
            state = NodeState.SUCCESS;
        }
        return state;
    }

    private void GetPointsOfInterest()
    {
        _interestPoints = new List<PointOfInterest>();
        Collider[] colliders = Physics.OverlapSphere(_transform.position, _searchRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("InterestObject"))
            {
                PointOfInterest poi = col.GetComponent<PointOfInterest>();
                if (poi != null)
                {
                    _interestPoints.Add(poi);
                }
            }
        }
    }

    private PointOfInterest GetHighestInterestPoint(List<PointOfInterest> points)
    {
        PointOfInterest bestPoint = null;
        float highestInterest = 0f;

        foreach (var point in points)
        {
            float interestValue = point.GetInterestValue(_transform);
            if (interestValue > highestInterest)
            {
                highestInterest = interestValue;
                bestPoint = point;
            }
        }
        return bestPoint;
    }
}
