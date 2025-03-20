using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public enum InterestType { Tree, Rock }
    public InterestType interestType;

    private Transform agentTransform;

    private float baseInterest;
    private float currentInterest;
    private float maxDistance = 50f; // Rango máximo de influencia
    private float resetTime = 120f; // 2 minutos
    private bool interestConsumed = false;

    private void Start()
    {
        baseInterest = interestType == InterestType.Tree ? 10f : 5f;
        currentInterest = baseInterest;
        agentTransform = FindObjectOfType<Beast_V3>().transform;
    }

    public float GetInterestValue(Transform agent)
    {
        if (!interestConsumed)
        {
            float distance = Vector3.Distance(transform.position, agent.position);
            currentInterest = baseInterest * Mathf.Clamp01(1 - (distance / maxDistance)); // Reduce interés con distancia
        }
        else
        {
            currentInterest = 0f;
        }
        return currentInterest;
    }

    public void ConsumeInterest()
    {
        currentInterest = 0;
        interestConsumed = true;
        StartCoroutine(ResetInterest());
    }

    private IEnumerator ResetInterest()
    {
        yield return new WaitForSeconds(resetTime);
        currentInterest = baseInterest;
        interestConsumed = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
        if (agentTransform != null) // Evita el error si no se ha encontrado el agente aún
        {
            if (!interestConsumed)
            {
                float distance = Vector3.Distance(transform.position, agentTransform.position);
                currentInterest = baseInterest * Mathf.Clamp01(1 - (distance / maxDistance)); // Reduce interés con distancia
            }
            else
            {
                currentInterest = 0f;
            }
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2, $"Interest: {currentInterest}");
        }
        else
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2, "Agent not found");
        }
    }
}
