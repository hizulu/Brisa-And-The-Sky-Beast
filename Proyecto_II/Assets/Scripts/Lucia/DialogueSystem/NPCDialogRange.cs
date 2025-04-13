using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogRange : MonoBehaviour
{
    public DialogManager dialogManager;
    public int startID;
    public int endID;

    private bool playerInRange = false;
    private bool dialogStarted = false;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        playerInput.UIPanelActions.Dialogue.started += OnInteract;        
    }

    private void OnDestroy()
    {
        playerInput.UIPanelActions.Dialogue.started -= OnInteract;
    }

    void Update()
    {
        //if (playerInRange && Input.GetKeyDown(KeyCode.E))
        //{
        //    OnInteract();
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Puedes hablar con el NPC");
        }
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Iniciando conversación");
        if (!playerInRange) return;

        if (!dialogStarted)
        {
            dialogManager.StartDialog(startID, endID); // Inicia el diálogo desde el ID inicial
            dialogStarted = true;
        }
        else
        {
            dialogManager.AdvanceDialog();  // Avanza al siguiente paso del diálogo
        }
    }
}
