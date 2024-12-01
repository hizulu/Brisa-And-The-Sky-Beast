using BBUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeastActionPlatform : MonoBehaviour
{
    private static bool actionPressed = false;
    static GameObject beast;
    private static Transform platformTransform;

    private void Awake()
    {
        beast = GameObject.FindGameObjectWithTag("Beast");
    }

    private void Update()
    {
        platformTransform = transform;
    }

    //When action is pressed
    public static void LinkBeast()
    {
        beast.transform.parent = platformTransform;
        NavMeshAgent agent = beast.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }
        Debug.Log("Objetos vinculados");
    }

    // When destination is reached
    public static void RemoveLink()
    {
        beast.transform.parent = null;
        NavMeshAgent agent = beast.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;

            // Asegúrate de sincronizar la posición del agente con el Transform.
            agent.Warp(beast.transform.position);
        }
        Debug.Log("Objetos desvinculados");
    }
}
