//using BBUnity.Actions;
using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BeastActionPlatform : MonoBehaviour
{
    private static bool actionPressed = false;
    static GameObject beast;
    private static Transform platformTransform;

    [SerializeField] private GameObject defaultPanel; // Panel de interacción
    //[SerializeField] private TextMeshProUGUI panelText; // Texto dentro del panel

    [SerializeField] private string panelMessage = "Para que la bestia se suba a la plataforma, llámala (Q) y cuando esté esperando sobre la plataforma pulsa TAB.";
    private bool isPlayerInRange = false; // Verifica si el jugador está dentro del área

    private void Awake()
    {
        beast = GameObject.FindGameObjectWithTag("Beast");
    }

    private void Start()
    {
        defaultPanel.SetActive(false);
    }

    private void Update()
    {
        platformTransform = transform;

        UpdatePanel();
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
        //BeastBasicMovement.GiveBeastFreedom();
        EndingTrigger.beastUp = true;
    }

    void UpdatePanel()
    {
        if (isPlayerInRange && !EndingTrigger.beastUp)
        {
            defaultPanel.SetActive(true);
            //panelText.optionText = panelMessage;
        }
        else
        {
            defaultPanel.SetActive(false);
        }
    }


    // Detecta si el jugador entra al trigger de la palanca
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("player entra");
        }
    }

    // Detecta si el jugador sale del trigger de la palanca
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("player SALE");
        }
    }
}
