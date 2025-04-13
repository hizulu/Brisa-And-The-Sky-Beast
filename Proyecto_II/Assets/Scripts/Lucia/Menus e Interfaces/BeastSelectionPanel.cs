#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
#endregion

/* NOMBRE CLASE: Beast Selection Panel
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 03/04/2025
 * DESCRIPCI�N: Script que se encarga de gestionar el panel de selecci�n de la Bestia.
 * VERSI�N: 1.0
 */

public class BeastSelectionPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject beastSelectionPanel;
    private bool beastPanelEnabled = false;

    private void Awake()
    {
        if (beastSelectionPanel == null)
            beastSelectionPanel = gameObject;
    }

    // M�todo para abrir y cerrar el panel de selecci�n de bestias
    public void OpenCloseBeastPanel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.name == "tab")
        {

            if (beastPanelEnabled)
            {
                beastSelectionPanel.SetActive(false);
                beastPanelEnabled = false;
            }
            else
            {
                beastSelectionPanel.SetActive(true);
                beastPanelEnabled = true;
            }

            // No est� funcionando.
            //EventsManager.TriggerSpecialEvent<bool>("PauseMode", inventoryEnabled);
            Time.timeScale = beastPanelEnabled ? 0f : 1f;
        }
    }

    //TODO Esto tengo que revisarlo
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Panel clicked!");
    }

    // LLamar a los eventos de cada personaje.
    public void AcariciarBestia()
    {
        EventsManager.TriggerNormalEvent("AcariciarBestia_Bestia");
        EventsManager.TriggerNormalEvent("AcariciarBestia_Player");
    }

    public void ClosePanel()
    {
        beastSelectionPanel.SetActive(false);
        beastPanelEnabled = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
