using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 28/04/2025
public class BeastEnemyDetectionTrigger : MonoBehaviour
{
    private Beast beast;

    private void Awake()
    {
        beast = GetComponentInParent<Beast>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            beast.OnEnemyEnter(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            beast.OnEnemyExit(other.gameObject);
        }
    }
}
