using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;
using System.Linq;

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
            playerInput.UIPanelActions.Dialogue.performed += OnContinueSignPerformed;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.UIPanelActions.Dialogue.performed -= OnContinueSignPerformed;
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

        // Elimina la condición !isTextActive que estaba aquí antes

        var lines = signsCSV.text.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Skip(1) // Saltar encabezado
            .Select(line => line.Trim());

        foreach (var line in lines)
        {
            var values = line.Split(new[] { ';' }, 2);
            if (values.Length >= 2 && int.TryParse(values[0], out int signId))
            {
                string text = values[1].Trim()
                    .Replace("\\n", "\n")
                    .Replace("\r\n", "\n")
                    .Replace("\r", "\n");
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
        UnlockPlayerControls();
    }

    private void LockPlayerControls()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0f;
        }
    }

    private void UnlockPlayerControls()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 300f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 300f;
        }
    }
}