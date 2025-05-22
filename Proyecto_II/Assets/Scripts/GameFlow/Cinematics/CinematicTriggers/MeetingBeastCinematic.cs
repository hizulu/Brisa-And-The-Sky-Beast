using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 22/05/2025
public class MeetingBeastCinematic : MonoBehaviour
{
    private bool hasBeenTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            CinematicsManager.Instance.PlayCinematic(0);
            Destroy(gameObject);
        }
    }
}
