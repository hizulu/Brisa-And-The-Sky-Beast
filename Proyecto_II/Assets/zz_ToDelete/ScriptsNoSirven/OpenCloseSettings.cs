using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenCloseSettings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;

    

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventsManager.TriggerNormalEvent("UIPanelOpened");
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        EventsManager.TriggerNormalEvent("UIPanelClosed");
    }

    public void OpenCloseSettingsPanel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.name == "escape")
        {
            if (settingsPanel.activeSelf)
            {
                CloseSettingsPanel();
            }
        }
    }
}
