using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;

public class SignTextManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject signPanel;
    public TextMeshProUGUI signText;
    [SerializeField] private Canvas hudCanvas;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Player Control")]
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    private CinemachinePOV cameraPOV;

    [Header("Data")]
    public TextAsset signsCSV;

    private Dictionary<int, string> signDictionary = new Dictionary<int, string>();
    private bool isTextActive = false;

    void Awake()
    {
        // Inicialización segura de componentes  
        if (playerCamera != null)
        {
            cameraPOV = playerCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        LoadSignTextFromCSV();
        signPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.UIPanelActions.DialogueContinue.performed += OnContinueSignPerformed;
        }
        else
        {
            Debug.LogWarning("PlayerInput no asignado en SignTextManager");
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.UIPanelActions.DialogueContinue.performed -= OnContinueSignPerformed;
        }
        else
        {
            Debug.LogWarning("PlayerInput no asignado en SignTextManager");
        }
    }

    private void OnContinueSignPerformed(InputAction.CallbackContext context)
    {
        if (!isTextActive) return;
        CloseSignPanel();
    }

    void LoadSignTextFromCSV()
    {
        if (signsCSV == null)
        {
            Debug.LogWarning("No se asignó archivo CSV de señales");
            return;
        }

        string[] lines = signsCSV.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(';');

            if (values.Length >= 2 && int.TryParse(values[0], out int signId))
            {
                string text = values[1].Trim().Replace("\\n", "\n");
                signDictionary[signId] = text;
            }
        }
    }

    public void ShowSignTextById(int signId)
    {
        if (isTextActive || !signDictionary.ContainsKey(signId)) return;

        isTextActive = true;
        signText.text = signDictionary[signId];
        signPanel.SetActive(true);

        if (hudCanvas != null)
        {
            hudCanvas.gameObject.SetActive(false);
        }

        Time.timeScale = 0f;

        // Bloquear controles
        LockPlayerControls();
    }

    public void CloseSignPanel()
    {
        if (!isTextActive) return;

        isTextActive = false;
        signPanel.SetActive(false);

        if (hudCanvas != null)
        {
            hudCanvas.gameObject.SetActive(true);
        }

        Time.timeScale = 1f;

        // Restaurar controles
        UnlockPlayerControls();
    }

    private void LockPlayerControls()
    {
        // Bloquear movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // Bloquear movimiento de cámara
        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0f;
        }
    }

    private void UnlockPlayerControls()
    {
        // Restaurar movimiento del jugador
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Restaurar movimiento de cámara
        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 300f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 300f;
        }
    }
}