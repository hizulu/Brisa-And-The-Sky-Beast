using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionableButtonMovesPlatform : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform platform; 
    [SerializeField] private Transform position1; 
    [SerializeField] private Transform position2; 

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f; // Velocidad de movimiento de la plataforma

    private bool isPlayerInRange = false; // Verifica si el jugador está dentro del área
    private bool isPlatformAtPosition1 = true; // Estado actual de la plataforma
    private bool isMoving = false; // Si la plataforma se está moviendo

    private void Update()
    {
        // Si la plataforma se está moviendo, desplázala a la posición correspondiente
        if (isMoving)
        {
            MovePlatform();
        }
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
