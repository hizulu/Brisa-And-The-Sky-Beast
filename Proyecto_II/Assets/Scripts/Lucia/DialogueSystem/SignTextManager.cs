using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;
using System.Linq; // Para el manejo más limpio del CSV

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
        Debug.Log("Bandera 1 - Awake iniciado");

        if (playerCamera != null)
        {
            cameraPOV = playerCamera.GetCinemachineComponent<CinemachinePOV>();
            Debug.Log("Bandera 2 - Componente POV obtenido");
        }

        LoadSignTextFromCSV();
        signPanel.SetActive(false);
        Debug.Log("Bandera 3 - Panel desactivado inicialmente");
    }

    private void OnEnable()
    {
        Debug.Log("Bandera 4 - OnEnable iniciado");
        if (playerInput != null)
        {
            playerInput.UIPanelActions.Dialogue.performed += OnContinueSignPerformed;
            Debug.Log("Bandera 5 - Evento de input suscrito");
        }
        else
        {
            Debug.LogWarning("PlayerInput no asignado en SignTextManager");
        }
    }

    private void OnDisable()
    {
        Debug.Log("Bandera 6 - OnDisable iniciado");
        if (playerInput != null)
        {
            playerInput.UIPanelActions.Dialogue.performed -= OnContinueSignPerformed;
            Debug.Log("Bandera 7 - Evento de input desuscrito");
        }
    }

    private void OnContinueSignPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Bandera 8 - Input detectado");
        if (!isTextActive)
        {
            Debug.Log("Bandera 9 - Panel no activo, ignorando input");
            return;
        }
        CloseSignPanel();
    }

    void LoadSignTextFromCSV()
    {
        Debug.Log("Bandera 10 - Cargando CSV");
        if (signsCSV == null)
        {
            Debug.LogWarning("No se asignó archivo CSV de señales");
            return;
        }

        // Divide el CSV en líneas y omite la primera (encabezado)
        var lines = signsCSV.text.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Skip(1)
            .Select(line => line.Trim());

        foreach (var line in lines)
        {
            Debug.Log($"Bandera 11 - Procesando línea: {line}");

            var values = line.Split(new[] { ';' }, 2); // Divide solo en el primer ;

            if (values.Length >= 2 && int.TryParse(values[0], out int signId))
            {
                string text = values[1].Trim();
                Debug.Log($"Bandera 12 - Antes de reemplazar: {text}");

                text = text.Replace("\\n", "\n")
                          .Replace("\r\n", "\n")
                          .Replace("\r", "\n");

                signDictionary[signId] = text;
                Debug.Log($"Bandera 13 - Texto procesado (ID {signId}): {text}");
            }
            else
            {
                Debug.LogWarning($"Bandera 14 - Formato inválido en línea: {line}");
            }
        }
        Debug.Log($"Bandera 15 - CSV cargado. Total entradas: {signDictionary.Count}");
    }

    public void ShowSignTextById(int signId)
    {
        Debug.Log($"Bandera 16 - Intentando mostrar signo ID: {signId}");

        if (isTextActive)
        {
            Debug.Log("Bandera 17 - Panel ya activo, ignorando");
            return;
        }

        if (!signDictionary.ContainsKey(signId))
        {
            Debug.LogWarning($"Bandera 18 - ID {signId} no encontrado en diccionario");
            return;
        }

        isTextActive = true;
        signText.text = signDictionary[signId];
        signPanel.SetActive(true);
        Debug.Log($"Bandera 19 - Panel activado con texto: {signDictionary[signId]}");

        if (hudCanvas != null)
        {
            hudCanvas.gameObject.SetActive(false);
            Debug.Log("Bandera 20 - HUD desactivado");
        }

        Time.timeScale = 0f;
        Debug.Log("Bandera 21 - Tiempo pausado");

        LockPlayerControls();
    }

    public void CloseSignPanel()
    {
        Debug.Log("Bandera 22 - Cerrando panel");
        if (!isTextActive) return;

        isTextActive = false;
        signPanel.SetActive(false);
        Debug.Log("Bandera 23 - Panel desactivado");

        if (hudCanvas != null)
        {
            hudCanvas.gameObject.SetActive(true);
            Debug.Log("Bandera 24 - HUD reactivado");
        }

        Time.timeScale = 1f;
        Debug.Log("Bandera 25 - Tiempo reanudado");

        UnlockPlayerControls();
    }

    private void LockPlayerControls()
    {
        Debug.Log("Bandera 26 - Bloqueando controles");
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
            Debug.Log("Bandera 27 - Movimiento del jugador bloqueado");
        }

        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0f;
            Debug.Log("Bandera 28 - Cámara bloqueada");
        }
    }

    private void UnlockPlayerControls()
    {
        Debug.Log("Bandera 29 - Desbloqueando controles");
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
            Debug.Log("Bandera 30 - Movimiento del jugador desbloqueado");
        }

        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 300f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 300f;
            Debug.Log("Bandera 31 - Cámara desbloqueada");
        }
    }
}