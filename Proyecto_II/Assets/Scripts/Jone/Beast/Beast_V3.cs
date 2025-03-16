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
        InvokeRepeating(nameof(FindBestInterestPoint), 0, 1f);
    }

    private void FindBestInterestPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        Debug.Log($"Buscando puntos de interés en radio: {searchRadius}");
        List<PointOfInterest> interestPoints = new List<PointOfInterest>();

        foreach (Collider col in colliders)
        {
            Debug.Log($"Detectado objeto: {col.name} con tag {col.tag}");
            if (col.CompareTag(interestTag))
            {
                PointOfInterest poi = col.GetComponent<PointOfInterest>();
                if (poi != null)
                {
                    interestPoints.Add(poi);
                    Debug.Log("Interest point added");
                }
            }
        }

        if (interestPoints.Count > 0)
        {
            currentTarget = GetHighestInterestPoint(interestPoints);
            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.transform.position);
                Debug.Log("New destination set");
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
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.transform.position) < 1.5f)
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
            Invoke(nameof(FindBestInterestPoint), 1f); // Espera un momento antes de buscar otro punto
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
