using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 20/04/2025
public class TEMPReviveBeast : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsManager.TriggerNormalEvent("ReviveBeast");
            Debug.Log("Evento de revivir bestia triggereado");
        }
    }
}
