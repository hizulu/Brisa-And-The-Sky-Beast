#region Bibliotecas
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cinemachine;
using System.Linq;
#endregion

/* 
 * NOMBRE CLASE: SignTextManager
 * AUTOR: Lucía García López
 * FECHA: 03/05/2025
 * DESCRIPCIÓN: Script que gestiona la visualización de texto en carteles en el juego. 
 *              Funciona con un archivo CSV que contiene las entradas de texto.
 * VERSIÓN: 1.0 Sistema de carteles inicial. Funcionamiento similar al de diálogos.
 */

public class SignTextManager : MonoBehaviour
{
    #region Variables
    [Header("UI References")]
    public GameObject signPanel;
    public TextMeshProUGUI signText;
    [SerializeField] private Canvas hudCanvas;

    [Header("Player Control")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    private CinemachinePOV cameraPOV;

    [Header("Data")]
    public TextAsset signsCSV;

    private Dictionary<int, string> signDictionary = new Dictionary<int, string>();
    private bool isTextActive = false;
    private PlayerInput playerInput;
    #endregion

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        if (playerCamera != null)
        {
            cameraPOV = playerCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        LoadSignTextFromCSV();
        signPanel.SetActive(false);
    }

    private void Update()
    {
        if (isTextActive && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseSignPanel();
        }
    }

    void LoadSignTextFromCSV()
    {
        if (signsCSV == null)
        {
            Debug.LogWarning("No se asignó archivo CSV de señales");
            return;
        }

        var lines = signsCSV.text.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Skip(1) // Saltar encabezado
            .Select(line => line.Trim());

        foreach (var line in lines)
        {
            var values = line.Split(';');
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
        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0f;
        }
    }

    private void UnlockPlayerControls()
    {
        if (cameraPOV != null)
        {
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 300f;
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 300f;
        }
    }
}