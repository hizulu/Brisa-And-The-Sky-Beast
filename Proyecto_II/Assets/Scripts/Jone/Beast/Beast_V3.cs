using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Jone Sainz Egea
// Tercera versión de la bestia, sistema de prioridades
// 16/03/2025
public class Beast_V3 : MonoBehaviour
{
    public float searchRadius = 10f;
    public string interestTag = "InterestObject";
    private NavMeshAgent agent;
    private PointOfInterest currentTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Invoke(nameof(FindBestInterestPoint), 1f);
    }

    private void FindBestInterestPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        List<PointOfInterest> interestPoints = new List<PointOfInterest>();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(interestTag))
            {
                PointOfInterest poi = col.GetComponent<PointOfInterest>();
                if (poi != null)
                {
                    interestPoints.Add(poi);
                }
            }
        }

            if (interestPoints.Count > 0)
            {
                currentTarget = GetHighestInterestPoint(interestPoints);

                if (currentTarget != null)
                {
                    agent.SetDestination(currentTarget.transform.position);
                    Debug.Log($"New destination set to: {currentTarget.transform.position}");
                }
            }
    }

    private PointOfInterest GetHighestInterestPoint(List<PointOfInterest> points)
    {
        PointOfInterest bestPoint = null;
        float highestInterest = 0f;

        foreach (var point in points)
        {
            float interestValue = point.GetInterestValue(transform);
            if (interestValue > highestInterest)
            {
                highestInterest = interestValue;
                bestPoint = point;
            }
        }
        return bestPoint;
    }

    private void Update()
    {
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < 5f)
        {
            InteractWithPoint();
        }
    }

    private void InteractWithPoint()
    {
        if (currentTarget != null)
        {
            Debug.Log($"Interacted with {currentTarget.name}, interest consumed.");
            currentTarget.ConsumeInterest();
            currentTarget = null;
            StartCoroutine(WaitAndSearch());
        }
    }
    private IEnumerator WaitAndSearch()
    {
        yield return new WaitForSeconds(1f); // Aquí habría que esperar a que termine la interacción
        Debug.Log("Looking for new destination");
        FindBestInterestPoint();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
