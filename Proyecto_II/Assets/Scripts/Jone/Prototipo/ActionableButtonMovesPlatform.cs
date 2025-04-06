using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/* NOMBRE CLASE: ActionableButtonMovesPlatform
 * AUTOR: Jone Sainz Egea
 * FECHA: 01/12/2024
 * DESCRIPCIÓN: script que cambia la posición de una plataforma con un interruptor accionable
 * VERSIÓN: 1.0 funcionamiento básico levantar plataforma
 *              1.1 mostrar text interactuar
 */

public class ActionableButtonMovesPlatform : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform platform; // La plataforma a mover
    [SerializeField] private Transform position1; // Posición inicial de la plataforma
    [SerializeField] private Transform position2; // Posición final de la plataforma
    [SerializeField] private GameObject interactionPanel; // Panel de interacción
    //[SerializeField] private TextMeshProUGUI interactionText; // Texto dentro del panel

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f; // Velocidad de movimiento de la plataforma
    //[SerializeField] private string interactMessage = "Pulsa \"E\" para interactuar."; // Mensaje por defecto

    private bool isPlayerInRange = false; // Verifica si el jugador está dentro del área
    private bool isPlatformAtPosition1 = false; // Estado actual de la plataforma
    private bool isMoving = false; // Si la plataforma se está moviendo

    private void Start()
    {
        interactionPanel.SetActive(false);
    }

    private void Update()
    {
        // Si la plataforma se está moviendo, desplázala a la posición correspondiente
        if (isMoving)
        {
            MovePlatform();
        }

        // Actualizar la visibilidad del panel de interacción
        UpdateInteractionPanel();
    }

    private void MovePlatform()
    {
        Transform targetPosition = isPlatformAtPosition1 ? position2 : position1;
        platform.position = Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        // Comprueba si la plataforma ha llegado al destino
        if (Vector3.Distance(platform.position, targetPosition.position) < 0.01f)
        {
            platform.position = targetPosition.position; // Asegurarse de que la posición sea exacta
            isMoving = false;
            BeastActionPlatform.RemoveLink();
        }
    }

    private void UpdateInteractionPanel()
    {
        // Mostrar el panel solo si el jugador está en rango y la plataforma no se está moviendo
        if (isPlayerInRange && !isMoving)
        {
            interactionPanel.SetActive(true);
            //interactionText.text = interactMessage;
        }
        else
        {
            interactionPanel.SetActive(false);
        }
    }

    // Método vinculado al Input System para detectar la tecla "E"
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && isPlayerInRange && !isMoving)
        {
            isPlatformAtPosition1 = !isPlatformAtPosition1; // Cambia el estado de la plataforma
            isMoving = true; // Inicia el movimiento
        }
    }

    // Detecta si el jugador entra al trigger de la palanca
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Detecta si el jugador sale del trigger de la palanca
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
