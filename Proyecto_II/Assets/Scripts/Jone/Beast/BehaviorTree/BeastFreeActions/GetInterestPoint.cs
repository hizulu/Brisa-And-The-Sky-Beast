using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GetInterestPoint : Node
{
    private Transform _transform;
    private List<PointOfInterest> _interestPoints;
    private float _searchRadius;
    private PointOfInterest _highestPoint;

    public GetInterestPoint(Transform transfrom, float searchRadius) : base()
    {
        _transform = transfrom;
        _searchRadius = searchRadius;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("GettingPointsOfInterest");
        GetPointsOfInterest();
        _highestPoint = GetHighestInterestPoint(_interestPoints);
        state = NodeState.RUNNING;
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

    //private void FindBestInterestPoint()
    //{
    //    interestInBrisa = GetInterestInBrisa();
    //    if (interestInBrisa > 50f)
    //    {
    //        Debug.Log("Brisa es el destino");
    //        interestedInBrisa = true;
    //    }
    //    else
    //    {
    //        GetPointsOfInterest();

    //        //if (interestPoints.Count > 0)
    //        //{
    //        //    //currentTarget = GetHighestInterestPoint(interestPoints);

    //        //    //if (currentTarget != null)
    //        //    //{
    //        //    //    agent.SetDestination(currentTarget.transform.position);
    //        //    //}
    //        //}
    //        //else if (!isWaiting)
    //        //{
    //        //    StartNewCoroutine(WaitAndSearch(10f));
    //        //}
    //    }
    //}
}
