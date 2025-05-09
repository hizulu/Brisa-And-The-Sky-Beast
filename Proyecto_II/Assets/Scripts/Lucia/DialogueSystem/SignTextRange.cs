#region Bibliotecas
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
#endregion

/*
 * NOMBRE CLASE: SignTextRange
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 03/05/2025
 * DESCRIPCI�N: Clase que gestiona el rango de interacci�n con un cartel. 
 *              Permite al jugador leer el texto del cartel al entrar en su rango y presionar la tecla "E".
 * VERSI�N: 1.0 Sistema de carteles inicial.
 */

public class SignTextRange : MonoBehaviour
{
    #region Variables
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
    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
        if (interactionPanel != null) interactionPanel.SetActive(false);
    }

    //Si el jugador entra en el rango del cartel, se activa el panel de interacci�n.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowInteractionPanel();
        }
    }

    //Si el jugador sale del rango del cartel, se desactiva el panel de interacci�n.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideInteractionPanel();
            if (signActive) CloseSign();
        }
    }

    //Si el jugador est� en rango y presiona la tecla de interacci�n, se activa o desactiva el cartel.
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

    // M�todo que se llama cuando el jugador presiona la tecla de interacci�n.
    private void ShowInteractionPanel()
    {
        if (interactionPanel != null)
        {
            interactionText.text = signPromptText;
            interactionPanel.SetActive(true);
            UpdatePanelPosition();
        }
    }

    // M�todo que se llama cuando el jugador suelta la tecla de interacci�n.
    private void HideInteractionPanel()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    // M�todo que actualiza la posici�n del panel de interacci�n en la pantalla.
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

    // M�todo que se llama cuando el jugador presiona la tecla de interacci�n.
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

    //M�todo para abrir el cartel.
    private void OpenSign()
    {
        if (signTextManager != null)
        {
            signTextManager.ShowSignTextById(signId);
            signActive = true;
            HideInteractionPanel();
        }
    }

    //M�todo para cerrar el cartel.
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