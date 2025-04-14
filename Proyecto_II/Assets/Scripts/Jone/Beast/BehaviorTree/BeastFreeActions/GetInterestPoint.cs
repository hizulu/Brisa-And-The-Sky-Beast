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
        // Inicializa lookForTarget si no existe
        if (!_blackboard.TryGetValue("lookForTarget", out bool lookForTarget))
        {
            lookForTarget = true;
            _blackboard.SetValue("lookForTarget", lookForTarget);
        }

        // Si no debe buscar, devuelve failure directamente
        if (!lookForTarget)
        {
            state = NodeState.FAILURE;
            return state;
        }

        // Comprueba si ya tiene un objetivo asignado
        PointOfInterest currentTarget = _blackboard.GetValue<PointOfInterest>("target");

        if (currentTarget != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        // Busca nuevos puntos de interés
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
