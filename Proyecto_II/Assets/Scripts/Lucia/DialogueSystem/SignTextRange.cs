using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class SignTextRange : MonoBehaviour
{
    [Header("Sign Configuration")]
    [SerializeField] private SignTextManager signTextManager;
    [SerializeField] private int signId;
    [SerializeField] private Vector3 panelOffset = new Vector3(0, 3f, 0);

    [Header("UI References")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private string signPromptText = "Presiona E para leer";

    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera playerCam;
    private CinemachinePOV camComponents;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference interactAction;

    private bool playerInRange = false;
    private bool signActive = false;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (playerCam != null) camComponents = playerCam.GetCinemachineComponent<CinemachinePOV>();

        if (interactionPanel != null) interactionPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed += OnSignInteract;
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnSignInteract;
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        if (playerInRange && interactionPanel.activeSelf)
        {
            UpdatePanelPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowInteractionPanel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideInteractionPanel();

            if (signActive)
            {
                CloseSign();
            }
        }
    }

    private void OnSignInteract(InputAction.CallbackContext context)
    {
        if (!playerInRange || signTextManager == null) return;

        if (context.control.device is Keyboard && context.control.name == "e")
        {
            ToggleSign();
        }
    }

    private void ShowInteractionPanel()
    {
        if (interactionPanel != null)
        {
            interactionText.text = signPromptText;
            interactionPanel.SetActive(true);
            UpdatePanelPosition();
        }
    }

    private void HideInteractionPanel()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    private void UpdatePanelPosition()
    {
        if (mainCamera != null && panelRectTransform != null)
        {
            // Calcular posición exacta encima del cartel
            Vector3 worldPosition = transform.position + panelOffset;

            // Ajustar para objetos con renderer
            if (TryGetComponent(out Renderer renderer))
            {
                worldPosition.y = renderer.bounds.max.y + 0.3f;
            }

            Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            panelRectTransform.position = screenPosition;
        }
    }

    private void ToggleSign()
    {
        if (!signActive)
        {
            OpenSign();
        }
        else
        {
            CloseSign();
        }
    }

    private void OpenSign()
    {
        signTextManager.ShowSignTextById(signId);
        signActive = true;
        HideInteractionPanel();

        if (playerCam != null)
        {
            playerCam.m_Lens.FieldOfView = 50f;
            LockCameraMovement();
        }
    }

    private void CloseSign()
    {
        signTextManager.CloseSignPanel();
        signActive = false;

        if (playerInRange)
        {
            ShowInteractionPanel();
        }

        if (playerCam != null)
        {
            playerCam.m_Lens.FieldOfView = 60f;
            UnlockCameraMovement();
        }
    }

    private void LockCameraMovement()
    {
        if (camComponents != null)
        {
            camComponents.m_HorizontalAxis.m_MaxSpeed = 0f;
            camComponents.m_VerticalAxis.m_MaxSpeed = 0f;
        }
    }

    private void UnlockCameraMovement()
    {
        if (camComponents != null)
        {
            camComponents.m_HorizontalAxis.m_MaxSpeed = 300f;
            camComponents.m_VerticalAxis.m_MaxSpeed = 300f;
        }
    }
}