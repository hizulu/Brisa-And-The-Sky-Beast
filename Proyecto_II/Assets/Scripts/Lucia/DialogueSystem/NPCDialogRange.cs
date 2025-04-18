using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using TMPro;

public class NPCDialogRange : MonoBehaviour
{
    public DialogManager dialogManager;
    public int startID;
    public int endID;

    [SerializeField] private string npcName;
    [SerializeField] private UINameNPC uiManager;

    private bool playerInRange = false;
    private bool dialogStarted = false;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private CinemachineVirtualCamera playerCam;
    private CinemachinePOV CamComponents;

    private void Awake()
    {
        EventsManager.CallNormalEvents("ResetCameraDialogue", ResumePlayerCamera);
        playerInput.UIPanelActions.Dialogue.started += OnInteract;   
        CamComponents = playerCam.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnDestroy()
    {
        EventsManager.StopCallNormalEvents("ResetCameraDialogue", ResumePlayerCamera);
        playerInput.UIPanelActions.Dialogue.started -= OnInteract;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            uiManager.ShowNPCPanelName(npcName, transform);
            Debug.Log("Puedes hablar con el NPC");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiManager.HideNPCPanelName();

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
            uiManager.HideNPCPanelName();
            dialogManager.StartDialog(startID, endID); // Inicia el diálogo desde el ID inicial
            dialogStarted = true;
            StartDialogCamera();
        }
        else
        {
            dialogManager.AdvanceDialog();  // Avanza al siguiente paso del diálogo
        }
    }

    private void StartDialogCamera()
    {
        playerInput.PlayerActions.Disable();

        playerCam.m_Lens.FieldOfView = 50f;
        StartCoroutine(TransitionCameraDialogue(-80f, 10f, 1f, true));
    }

    private void ResumePlayerCamera()
    {
        playerCam.m_Lens.FieldOfView = 60f;
        StartCoroutine(TransitionCameraDialogue(0f, 0f, 1f, false));

        playerInput.PlayerActions.Enable();
    }

    private IEnumerator TransitionCameraDialogue(float _horitontalAxis, float _verticalAxis, float _duration, bool _isDialogueActive)
    {
        float startPosX = CamComponents.m_HorizontalAxis.Value;
        float startPosY = CamComponents.m_VerticalAxis.Value;

        float elapsed = 0f;

        LockMovementCamera();

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float transitionCam = Mathf.SmoothStep(0, 1, elapsed / _duration);

            CamComponents.m_HorizontalAxis.Value = Mathf.LerpAngle(startPosX, _horitontalAxis, transitionCam);
            CamComponents.m_VerticalAxis.Value = Mathf.LerpAngle(startPosY, _verticalAxis, transitionCam);

            yield return null;
        }

        CamComponents.m_HorizontalAxis.Value = _horitontalAxis;
        CamComponents.m_VerticalAxis.Value = _verticalAxis;

        if (_isDialogueActive)
            LockMovementCamera();
        else
            UnLockMovementCamera();
    }

    private void LockMovementCamera()
    {
        CamComponents.m_HorizontalAxis.m_MaxSpeed = 0f;
        CamComponents.m_VerticalAxis.m_MaxSpeed = 0f;
    }

    private void UnLockMovementCamera()
    {
        CamComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
        CamComponents.m_VerticalAxis.m_MaxSpeed = 300f;
    }
}
