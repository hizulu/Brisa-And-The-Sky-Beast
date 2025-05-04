using UnityEngine;
using UnityEngine.InputSystem;

public class MapManager : MonoBehaviour
{
    [Header("Map Configuration")]
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private Camera mapCamera;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        // Verificación más robusta del panel
        if (mapPanel == null)
        {
            Debug.LogError("mapPanel no asignado en el Inspector!");
            return;
        }
        mapPanel.SetActive(false);

    }

    public void OpenCloseMapPanel(InputAction.CallbackContext context)
    {
        if (!context.performed || mapPanel == null) return;

        if (context.control.name == "m")
        {
            if (mapPanel.activeSelf)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }
    }

    public void ClosePanel()
    {
        mapPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventsManager.TriggerNormalEvent("UIPanelClosed");
        Debug.Log("Map closed");
    }

    private void OpenPanel()
    {
        mapPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        Debug.Log("Map opened");
    }
}