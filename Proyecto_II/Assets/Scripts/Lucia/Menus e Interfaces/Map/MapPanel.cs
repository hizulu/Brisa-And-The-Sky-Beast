using UnityEngine;
using UnityEngine.InputSystem;

public class MapPanel : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private Camera mapCamera;

    private PlayerInput playerInput;
    private InputAction openMapAction;
    private bool mapEnabled = false;

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();

        // Asegurarse de que el PlayerInput está asignado
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found!");
            return;
        }

        openMapAction = playerInput.UIPanelActions.Map;
        openMapAction.performed += OpenCloseMapPanel;
    }

    private void OnDisable()
    {
        if (openMapAction != null)
            openMapAction.performed -= OpenCloseMapPanel;
    }

    public void OpenCloseMapPanel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (mapEnabled)
            ClosePanel();
        else
            OpenPanel();

        // Control de tiempo
        Time.timeScale = mapEnabled ? 0f : 1f;
    }

    public void OpenPanel()
    {
        if (mapPanel == null)
        {
            Debug.LogError("mapPanel is null! Please assign it in the inspector.");
            return;
        }

        mapPanel.SetActive(true);
        mapEnabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EventsManager.TriggerNormalEvent("UIPanelOpened");
    }

    public void ClosePanel()
    {
        if (mapPanel == null)
        {
            Debug.LogError("mapPanel is null! Please assign it in the inspector.");
            return;
        }

        mapPanel.SetActive(false);
        mapEnabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EventsManager.TriggerNormalEvent("UIPanelClosed");
    }
}
