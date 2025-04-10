using UnityEngine;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;  // Si el jugador está dentro del área de trigger
    private bool isCoroutineRunning = false;  // Para evitar múltiples corutinas a la vez
    private DialogueManager dialogueManager;  // Referencia al DialogueManager

    private void Start()
    {
        // Buscar el DialogueManager en la escena solo una vez
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra en el área de activación
        if (other.CompareTag("Player") && !isCoroutineRunning)
        {
            isPlayerInRange = true;  // El jugador está dentro del área
            StartCoroutine(WaitForInteraction());  // Iniciar la espera de interacción
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale del área de activación
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // El jugador salió del área
            StopAllCoroutines();  // Detener todas las corutinas, por si quedaba alguna pendiente
            isCoroutineRunning = false;  // Resetear el flag de la corutina
        }
    }

    private IEnumerator WaitForInteraction()
    {
        isCoroutineRunning = true;  // Marcar que la corutina está en ejecución

        // Esperar hasta que el jugador presione E
        while (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))  // Cuando el jugador presiona E
            {
                StartDialogue();  // Iniciar el diálogo
                yield break;  // Terminar la corutina para no seguir verificando
            }
            yield return null;  // Esperar el siguiente frame
        }

        isCoroutineRunning = false;  // Corutina terminada
    }

    private void StartDialogue()
    {
        // Verificar si el DialogueManager está asignado
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(0);  // Llamar al método StartDialogue() con el ID del diálogo (0 en este caso)
            dialogueManager.EnableDialoguePanel(true);  // Activar el panel de diálogo
        }
    }
}
