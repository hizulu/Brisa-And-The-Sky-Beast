using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Jone Sainz Egea
// 18/04/2025

// Interfaz común para las zonas en las que la bestia puede realizar una acción específica
public interface IBeastActionable
{
    void OnBeast();
}

// Clase abstracta abierta a extensiones, permitiendo nuevos acciones golpeables
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
            // TODO: beast action button is available
            Debug.Log("Beast entered");
            beastIsIn = true;
            beast.blackboard.SetValue("isInActionZone", true);
            beast.blackboard.SetValue("isOptionAction", true);
            EventsManager.TriggerNormalEvent("BeastActionableEntered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beast"))
        {
            // TODO: beast action button is unavailable
            Debug.Log("Beast left");
            beastIsIn = false;
            beast.blackboard.SetValue("isInActionZone", false);
            beast.blackboard.SetValue("isOptionAction", false);
            EventsManager.TriggerNormalEvent("BeastActionableExited");
        }
    }

    public abstract void OnBeast();
}
