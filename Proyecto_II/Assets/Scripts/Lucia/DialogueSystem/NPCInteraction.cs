using UnityEngine;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;  // Si el jugador est� dentro del �rea de trigger
    private bool isCoroutineRunning = false;  // Para evitar m�ltiples corutinas a la vez
    private DialogueManager dialogueManager;  // Referencia al DialogueManager

    private void Start()
    {
        // Buscar el DialogueManager en la escena solo una vez
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra en el �rea de activaci�n
        if (other.CompareTag("Player") && !isCoroutineRunning)
        {
            isPlayerInRange = true;  // El jugador est� dentro del �rea
            StartCoroutine(WaitForInteraction());  // Iniciar la espera de interacci�n
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale del �rea de activaci�n
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // El jugador sali� del �rea
            StopAllCoroutines();  // Detener todas las corutinas, por si quedaba alguna pendiente
            isCoroutineRunning = false;  // Resetear el flag de la corutina
        }
    }

    private IEnumerator WaitForInteraction()
    {
        isCoroutineRunning = true;  // Marcar que la corutina est� en ejecuci�n

        // Esperar hasta que el jugador presione E
        while (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))  // Cuando el jugador presiona E
            {
                StartDialogue();  // Iniciar el di�logo
                yield break;  // Terminar la corutina para no seguir verificando
            }
            yield return null;  // Esperar el siguiente frame
        }

        isCoroutineRunning = false;  // Corutina terminada
    }

    private void StartDialogue()
    {
        // Verificar si el DialogueManager est� asignado
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(0);  // Llamar al m�todo StartDialogue() con el ID del di�logo (0 en este caso)
            dialogueManager.EnableDialoguePanel(true);  // Activar el panel de di�logo
        }
    }
}
