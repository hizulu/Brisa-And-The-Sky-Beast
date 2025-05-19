#region Bibliotecas
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using TMPro;
#endregion

/* NOMBRE CLASE: NPCDialogRange
 * AUTORES: Lucía García López y Sara Yue Madruga Martín
 * FECHA: 10/04/2025
 * DESCRIPCION: Clase que gestiona el rango de diálogo con un NPC. Un jugador puede interactuar con el NPC al entrar en su rango y presionar la tecla "E".
 *              Cada NPC tiene un ID de inicio y un ID de fin para el diálogo, lo que permite gestionar múltiples diálogos.
 *              Funciona con un archivo CSV que contiene las entradas de diálogo.
 * VERSION: 1.0 Lucia: Sistema de diálogos inicial.
 * 1.1 Sara: Cambio a New Input System.
 * 1.2 Sara: Añadido el nombre del NPC en el panel de interacción y bloqueo de movimiento de la cámara durante el diálogo.
 * 1.3 Lucia: Mejoras generales de funcionamiento
 */

public class NPCDialogRange : MonoBehaviour
{
    #region Variables
    [Header("Dialog Configuration")]
    public DialogManager dialogManager;
    public int startID;
    public int endID;

    [Header("UI References")]
    [SerializeField] private string npcName;
    [SerializeField] private UINameNPC uiManager;

    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera playerCam;
    private PlayerInput playerInput;

    private bool playerInRange = false;
    private bool dialogStarted = false;
    private CinemachinePOV camComponents;
    #endregion

    private void Awake()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        EventsManager.CallNormalEvents("ResetCameraDialogue", ResumePlayerCamera);
        camComponents = playerCam.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedEnable());
    }

    private IEnumerator DelayedEnable()
    {
        // Espera un frame para asegurar que todo está inicializado
        yield return null;

        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput no encontrado en la escena!");
                yield break;
            }
        }

        playerInput.UIPanelActions.Dialogue.started += OnInteract;
    }

    private void OnDestroy()
    {
        EventsManager.StopCallNormalEvents("ResetCameraDialogue", ResumePlayerCamera);
        playerInput.UIPanelActions.Dialogue.started -= OnInteract;
    }

    // Método que se llama cuando el jugador entra en el rango del NPC. 
    // Muestra el nombre del NPC en el panel de interacción.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;        
            uiManager.ShowNPCPanelName(npcName, transform);
            Debug.Log("Puedes hablar con el NPC");
        }
    }

    // Método que se llama cuando el jugador sale del rango del NPC.
    // Oculta el panel de interacción y cierra el diálogo si estaba abierto.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiManager.HideNPCPanelName();

            if (dialogStarted)
            {
                dialogManager.ForceCloseDialog(); // Cierra el diálogo si estaba abierto
                dialogStarted = false;
            }
        }
    }

    // Método que se llama cuando el jugador presiona la tecla "E" para interactuar con el NPC.
    private void OnInteract(InputAction.CallbackContext context)
    {
        // Verificar si fue E (Keyboard)
        bool isKeyboard = context.control.device is Keyboard && context.control.name == "e";

        if (!isKeyboard) return;;
        if (!playerInRange) return;

        if (!dialogStarted)
        {
            StartDialogue(); // Inicia el diálogo
        }
    }

    // Método que inicia el diálogo con el NPC.
    private void StartDialogue()
    {
        EventsManager.TriggerNormalEvent("NPCStartTalk");
        uiManager.HideNPCPanelName();
        dialogManager.StartDialog(startID, endID);
        dialogStarted = true;
        StartDialogCamera();
    }

    //Método para desactivar las acciones del jugador y animar un movimiento de la cámara.
    private void StartDialogCamera()
    {
        playerInput.PlayerActions.Disable();
        playerCam.m_Lens.FieldOfView = 50f;
        StartCoroutine(TransitionCameraDialogue(-80f, 10f, 1f, true));
    }

    // Método para reanudar las acciones del jugador y restaurar la cámara a su posición original.
    private void ResumePlayerCamera()
    {
        if (this == null) return;

        EventsManager.TriggerNormalEvent("NPCIdle");
        playerCam.m_Lens.FieldOfView = 60f;
        StartCoroutine(TransitionCameraDialogue(0f, 0f, 1f, false));
        playerInput.PlayerActions.Enable();
        dialogStarted = false; // Permite volver a iniciar el diálogo si el jugador sigue en rango
        if (playerInRange)
        {
            uiManager.ShowNPCPanelName(npcName, transform);
        }
    }

    // Método para animar la cámara durante el diálogo.
    private IEnumerator TransitionCameraDialogue(float horizontalAxis, float verticalAxis, float duration, bool isDialogueActive)
    {
        float startPosX = camComponents.m_HorizontalAxis.Value;
        float startPosY = camComponents.m_VerticalAxis.Value;

        float elapsed = 0f;

        LockMovementCamera();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float transitionCam = Mathf.SmoothStep(0, 1, elapsed / duration);

            camComponents.m_HorizontalAxis.Value = Mathf.LerpAngle(startPosX, horizontalAxis, transitionCam);
            camComponents.m_VerticalAxis.Value = Mathf.LerpAngle(startPosY, verticalAxis, transitionCam);

            yield return null;
        }

        camComponents.m_HorizontalAxis.Value = horizontalAxis;
        camComponents.m_VerticalAxis.Value = verticalAxis;

        if (isDialogueActive)
            LockMovementCamera();
        else
            UnLockMovementCamera();
    }

    // Método para bloquear el movimiento de la cámara durante el diálogo.
    private void LockMovementCamera()
    {
        camComponents.m_HorizontalAxis.m_MaxSpeed = 0f;
        camComponents.m_VerticalAxis.m_MaxSpeed = 0f;
    }

    //Método para desbloquear el movimiento de la cámara después del diálogo.
    private void UnLockMovementCamera()
    {
        camComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
        camComponents.m_VerticalAxis.m_MaxSpeed = 300f;
    }
}
