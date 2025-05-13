using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.Rendering;

// Jone Sainz Egea
// 05/04/2025
// Nodo que busca el punto de mayor interés, nunca falla, si no encuentra simplemente no hay target
    // 19/04/2025 Añadido interés en Brisa
public class GetInterestPoint : Node
{
    private Blackboard _blackboard;
    private Beast _beast;
    private Transform _playerTransform;
    private float _searchRadius;

    private List<PointOfInterest> _interestPoints;

    private float baseInterestInBrisa = 1f;
    private float growthFactorInterestInBrisa = 0.05f;


    public GetInterestPoint(Beast beast, Transform playerTransform, float searchRadius)
    {
        _beast = beast;
        _playerTransform = playerTransform;
        _searchRadius = searchRadius;

        _blackboard = _beast.blackboard;
    }

    public override NodeState Evaluate()
    {
        // Si ya tiene un objetivo válido, no hace falta buscar otro
        if (_blackboard.HasKey("target"))
        {
            state = NodeState.SUCCESS;
            return state;
        }

        // Buscar nuevos puntos de interés
        _interestPoints = GetPointsOfInterest();
        PointOfInterest bestPoint = GetHighestInterestPoint(_interestPoints);     

        if (bestPoint != null)
        {
            CompareWithInterestInBrisa(bestPoint);           
            _blackboard.SetValue("lookForTarget", false); // Ya ha encontrado un objetivo
            state = NodeState.SUCCESS;
        }
        else if (GetInterestInBrisa() > 10) // No hay puntos de interés y Brisa está lejos
        {
            Debug.Log("No interest points and Brisa is far");
        }

        return state;
    }

    private void CompareWithInterestInBrisa(PointOfInterest bestPoint)
    {
        float interestPointValue = bestPoint.GetInterestValue(_beast.transform);
        float interestInBrisa = GetInterestInBrisa();
        if (interestInBrisa > interestPointValue)
        {
            Debug.Log("Brisa is more interesting");
            _blackboard.SetValue("target", _playerTransform);
        }
        else
        {
            Debug.Log("Point is more interesting");
            _blackboard.SetValue("target", bestPoint);
        }
    }

    private float GetInterestInBrisa()
    {
        float distance = Vector3.Distance(_beast.transform.position, _playerTransform.position);
        return baseInterestInBrisa * Mathf.Exp(growthFactorInterestInBrisa * distance); // Aumento exponencial del interés en Brisa conforme se aleja    
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
