using System.Collections;
using UnityEngine;

// Jone Sainz Egea
// 16/03/2025
public class PointOfInterest : MonoBehaviour
{
    public enum InterestType { Tree, Rock, Flower }
    public InterestType interestType;

    private Transform agentTransform;

    private float baseInterest;
    private float currentInterest;
    private float maxDistance = 50f; // Rango máximo de influencia
    private float resetTime = 60f; // 1 minuto
    private bool interestConsumed = false;

    private void Start()
    {
        baseInterest = GetBaseInterest();
        currentInterest = baseInterest;
        agentTransform = FindObjectOfType<Beast>().transform;
    }

    private float GetBaseInterest()
    {
        switch (interestType)
        {
            case InterestType.Tree:
                return 10f;
            case InterestType.Rock:
                return 5f;
            case InterestType.Flower:
                return 2f;
            default:
                return 1f;
        }
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
        if (agentTransform != null)
        {
            if (!interestConsumed)
            {
                float distance = Vector3.Distance(transform.position, agentTransform.position);
                currentInterest = baseInterest * Mathf.Clamp01(1 - (distance / maxDistance));
            }
            else
            {
                currentInterest = 0f;
            }
            //UnityEditor.Handles.Label(transform.position + Vector3.up * 2, $"Interest: {currentInterest}");
        }
        else
        {
            //UnityEditor.Handles.Label(transform.position + Vector3.up * 2, "Agent not found");
        }
    }
}
