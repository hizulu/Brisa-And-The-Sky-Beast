using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Jone Sainz Egea
// 18/04/2025

// Interfaz común para las zonas en las que la bestia puede realizar una acción específica
public interface IBeastActionable
{
    bool OnBeast();
}

// Clase abstracta abierta a extensiones, permitiendo nuevos acciones de Bestia
public abstract class BeastActionable : MonoBehaviour, IBeastActionable
{
    protected bool beastIsIn = false;
    protected Beast beast;
    private void Awake()
    {
        beast = FindObjectOfType<Beast>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beast"))
        {
            // Debug.Log("Beast entered");
            beastIsIn = true;
            beast.blackboard.SetValue("isInActionZone", true);
            EventsManager.TriggerNormalEvent("BeastActionableEntered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beast"))
        {
            // Debug.Log("Beast left");
            beastIsIn = false;
            beast.blackboard.SetValue("isInActionZone", false);
            EventsManager.TriggerNormalEvent("BeastActionableExited");
        }
    }

    public abstract bool OnBeast();
}
