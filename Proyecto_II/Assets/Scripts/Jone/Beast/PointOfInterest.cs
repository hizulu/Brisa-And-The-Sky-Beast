using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public enum InterestType { Tree, Rock }
    public InterestType interestType;

    private float baseInterest;
    private float currentInterest;
    private float maxDistance = 10f; // Rango máximo de influencia
    private float resetTime = 120f; // 2 minutos

    private void Start()
    {
        baseInterest = interestType == InterestType.Tree ? 10f : 5f;
        currentInterest = baseInterest;
    }

    public float GetInterestValue(Transform agent)
    {
        float distance = Vector3.Distance(transform.position, agent.position);
        float interest = baseInterest * Mathf.Clamp01(1 - (distance / maxDistance)); // Reduce interés con distancia
        return interest;
    }

    public void ConsumeInterest()
    {
        currentInterest = 0;
        StartCoroutine(ResetInterest());
    }

    private IEnumerator ResetInterest()
    {
        yield return new WaitForSeconds(resetTime);
        currentInterest = baseInterest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 2, $"Interest: {currentInterest}");
    }
}
