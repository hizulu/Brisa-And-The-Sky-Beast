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

    private bool playerInRange = false;
    private bool signActive = false;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (interactionPanel != null) interactionPanel.SetActive(false);
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
            if (signActive) CloseSign();
        }
    }

    private void Update()
    {
        if (playerInRange && interactionPanel.activeSelf)
        {
            UpdatePanelPosition();

            // Manejo de input directo en Update para mayor confiabilidad
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                ToggleSign();
            }
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
            Vector3 worldPosition = transform.position + panelOffset;
            if (TryGetComponent(out Renderer renderer))
            {
                worldPosition.y = renderer.bounds.max.y + 0.3f;
            }
            panelRectTransform.position = mainCamera.WorldToScreenPoint(worldPosition);
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
        if (signTextManager != null)
        {
            signTextManager.ShowSignTextById(signId);
            signActive = true;
            HideInteractionPanel();
        }
    }

    private void CloseSign()
    {
        if (signTextManager != null)
        {
            signTextManager.CloseSignPanel();
            signActive = false;
            if (playerInRange) ShowInteractionPanel();
        }
    }
}