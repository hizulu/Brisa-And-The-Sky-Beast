using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using TMPro;

public class NPCDialogRange : MonoBehaviour
{
    [Header("Dialog Configuration")]
    public DialogManager dialogManager;
    public int startID;
    public int endID;

    [Header("UI References")]
    [SerializeField] private string npcName;
    [SerializeField] private UINameNPC uiManager;

    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private PlayerInput playerInput;

    private bool playerInRange = false;
    private bool dialogStarted = false;
    private CinemachinePOV camComponents;

    private void Awake()
    {
        EventsManager.CallNormalEvents("ResetCameraDialogue", ResumePlayerCamera);
        camComponents = playerCam.GetCinemachineComponent<CinemachinePOV>();

        // Configurar el input
        playerInput.UIPanelActions.Dialogue.started += OnInteract;
    }

    private void OnDestroy()
    {
        EventsManager.StopCallNormalEvents("ResetCameraDialogue", ResumePlayerCamera);
        playerInput.UIPanelActions.Dialogue.started -= OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            uiManager.ShowNPCPanelName(npcName, transform);
            Debug.Log("Puedes hablar con el NPC");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiManager.HideNPCPanelName();

            if (dialogStarted)
            {
                dialogManager.ForceCloseDialog();
                dialogStarted = false;
            }
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Verificar si fue E (Keyboard) o Click Izquierdo (Mouse)
        bool isKeyboard = context.control.device is Keyboard && context.control.name == "e";
        bool isMouse = context.control.device is Mouse && context.control.name == "leftButton";

        if (!isKeyboard && !isMouse) return;

        Debug.Log("Iniciando conversación");
        if (!playerInRange) return;

        if (!dialogStarted)
        {
            StartDialogue();
        }
        //else
        //{
        //    dialogManager.AdvanceDialog();
        //}
    }

    private void StartDialogue()
    {
        uiManager.HideNPCPanelName();
        dialogManager.StartDialog(startID, endID);
        dialogStarted = true;
        StartDialogCamera();
    }

    private void StartDialogCamera()
    {
        playerInput.PlayerActions.Disable();
        playerCam.m_Lens.FieldOfView = 50f;
        StartCoroutine(TransitionCameraDialogue(-80f, 10f, 1f, true));
    }

    private void ResumePlayerCamera()
    {
        if (this == null) return;

        playerCam.m_Lens.FieldOfView = 60f;
        StartCoroutine(TransitionCameraDialogue(0f, 0f, 1f, false));
        playerInput.PlayerActions.Enable();
    }

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

    private void LockMovementCamera()
    {
        camComponents.m_HorizontalAxis.m_MaxSpeed = 0f;
        camComponents.m_VerticalAxis.m_MaxSpeed = 0f;
    }

    private void UnLockMovementCamera()
    {
        camComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
        camComponents.m_VerticalAxis.m_MaxSpeed = 300f;
    }
}