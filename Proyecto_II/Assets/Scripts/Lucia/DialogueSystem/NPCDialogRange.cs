using UnityEngine;

public class NPCDialogRange : MonoBehaviour
{
    public DialogManager dialogManager;
    public int startID;
    public int endID;

    private bool playerInRange = false;
    private bool dialogStarted = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (dialogStarted)
            {
                dialogManager.ForceCloseDialog(); // Cierra el diálogo cuando el jugador sale del rango
                dialogStarted = false;
            }
        }
    }

    public void OnInteract()
    {
        if (!playerInRange) return;

        if (!dialogStarted)
        {
            dialogManager.StartDialog(startID, endID);  // Inicia el diálogo si no ha comenzado
            dialogStarted = true;
        }
        else
        {
            dialogManager.AdvanceDialog();  // Avanza al siguiente paso del diálogo
        }
    }
}
