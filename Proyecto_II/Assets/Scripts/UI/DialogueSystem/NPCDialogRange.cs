#region Bibliotecas
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using TMPro;
#endregion

/* NOMBRE CLASE: NPCDialogRange
 * AUTORES: Luc�a Garc�a L�pez y Sara Yue Madruga Mart�n
 * FECHA: 10/04/2025
 * DESCRIPCION: Clase que gestiona el rango de di�logo con un NPC. Un jugador puede interactuar con el NPC al entrar en su rango y presionar la tecla "E".
 *              Cada NPC tiene un ID de inicio y un ID de fin para el di�logo, lo que permite gestionar m�ltiples di�logos.
 *              Funciona con un archivo CSV que contiene las entradas de di�logo.
 * VERSION: 1.0 Lucia: Sistema de di�logos inicial.
 * 1.1 Sara: Cambio a New Input System.
 * 1.2 Sara: A�adido el nombre del NPC en el panel de interacci�n y bloqueo de movimiento de la c�mara durante el di�logo.
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
        // Espera un frame para asegurar que todo est� inicializado
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

    // M�todo que se llama cuando el jugador entra en el rango del NPC. 
    // Muestra el nombre del NPC en el panel de interacci�n.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;        
            uiManager.ShowNPCPanelName(npcName, transform);
            Debug.Log("Puedes hablar con el NPC");
        }
    }

    // M�todo que se llama cuando el jugador sale del rango del NPC.
    // Oculta el panel de interacci�n y cierra el di�logo si estaba abierto.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiManager.HideNPCPanelName();

            if (dialogStarted)
            {
                dialogManager.ForceCloseDialog(); // Cierra el di�logo si estaba abierto
                dialogStarted = false;
            }
        }
    }

    // M�todo que se llama cuando el jugador presiona la tecla "E" para interactuar con el NPC.
    private void OnInteract(InputAction.CallbackContext context)
    {
        // Verificar si fue E (Keyboard)
        bool isKeyboard = context.control.device is Keyboard && context.control.name == "e";

        if (!isKeyboard) return;;
        if (!playerInRange) return;

        if (!dialogStarted)
        {
            StartDialogue(); // Inicia el di�logo
        }
    }

    // M�todo que inicia el di�logo con el NPC.
    private void StartDialogue()
    {
        EventsManager.TriggerNormalEvent("NPCStartTalk");
        uiManager.HideNPCPanelName();
        dialogManager.StartDialog(startID, endID);
        dialogStarted = true;
        StartDialogCamera();
    }

    //M�todo para desactivar las acciones del jugador y animar un movimiento de la c�mara.
    private void StartDialogCamera()
    {
        playerInput.PlayerActions.Disable();
        playerCam.m_Lens.FieldOfView = 50f;
        StartCoroutine(TransitionCameraDialogue(-80f, 10f, 1f, true));
    }

    // M�todo para reanudar las acciones del jugador y restaurar la c�mara a su posici�n original.
    private void ResumePlayerCamera()
    {
        if (this == null) return;

        EventsManager.TriggerNormalEvent("NPCIdle");
        playerCam.m_Lens.FieldOfView = 60f;
        StartCoroutine(TransitionCameraDialogue(0f, 0f, 1f, false));
        playerInput.PlayerActions.Enable();
        dialogStarted = false; // Permite volver a iniciar el di�logo si el jugador sigue en rango
        if (playerInRange)
        {
            uiManager.ShowNPCPanelName(npcName, transform);
        }
    }

    // M�todo para animar la c�mara durante el di�logo.
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

    // M�todo para bloquear el movimiento de la c�mara durante el di�logo.
    private void LockMovementCamera()
    {
        camComponents.m_HorizontalAxis.m_MaxSpeed = 0f;
        camComponents.m_VerticalAxis.m_MaxSpeed = 0f;
    }

    //M�todo para desbloquear el movimiento de la c�mara despu�s del di�logo.
    private void UnLockMovementCamera()
    {
        camComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
        camComponents.m_VerticalAxis.m_MaxSpeed = 300f;
    }
}
