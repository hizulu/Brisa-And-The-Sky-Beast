using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapManager : MonoBehaviour
{
    [Header("Map Configuration")]
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private Camera mapCamera;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;

    private bool mapEnabled = false;

    private void Awake()
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found!");
            return;
        }
        if (playerInput != null)
        {
            playerInput.UIPanelActions.Map.performed += OpenCloseMapPanel;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.UIPanelActions.Map.performed -= OpenCloseMapPanel;
        }
    }

    public void OpenCloseMapPanel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (mapEnabled)
            ClosePanel();
        else
            OpenPanel();

        Time.timeScale = mapEnabled ? 0f : 1f;
    }

    public void OpenPanel()
    {
        if (mapPanel == null)
        {
            Debug.LogError("mapManager is null! Please assign it in the inspector.");
            return;
        }

        if (!mapPanel.activeSelf)
        {
            mapPanel.SetActive(true);
            mapEnabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            EventsManager.TriggerNormalEvent("UIPanelOpened");
        }
    }

    public void ClosePanel()
    {
        if (mapPanel == null)
        {
            Debug.LogError("mapManager is null! Please assign it in the inspector.");
            return;
        }

        if (mapPanel.activeSelf)
        {
            mapPanel.SetActive(false);
            mapEnabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            EventsManager.TriggerNormalEvent("UIPanelClosed");
        }
    }
}
