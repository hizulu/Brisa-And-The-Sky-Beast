#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
#endregion

/* NOMBRE CLASE: Beast Selection Panel
 * AUTORES: Lucía García López, Sara Yue Madruga Martín y Jone Sain Egea
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar el panel de selección de la Bestia.
 * VERSIÓN: 1.0 Lucía: Sistema de selección de acciones de la bestia inicial.
 * 
 */

public class BeastSelectionPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject beastSelectionPanel;
    [SerializeField] private Beast beast;
    private bool beastPanelEnabled = false;

    private void Awake()
    {
        if (beastSelectionPanel == null)
            beastSelectionPanel = gameObject;
    }

    // Método para abrir y cerrar el panel de selección de bestias
    public void OpenCloseBeastPanel(InputAction.CallbackContext context)
    {
        Debug.Log("Detecta acción openclosebeast");
        if (!context.performed)
            return;

        if (!beast.IsPlayerWithinInteractionDistance())
        {
            ClosePanel();
            Debug.Log("No va a abrir el menú");
            return;
        }

        if (context.control.name == "tab")
        {
            Debug.Log("Debería funcionar abrir menú");
            if (beastPanelEnabled)
                ClosePanel();
            else
                OpenPanel();

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
    public void SanarBestia()
    {
        EventsManager.TriggerNormalEvent("SanarBestia_Bestia");
        EventsManager.TriggerNormalEvent("SanarBestia_Player");
    }

    public void MontarBestia()
    {
        EventsManager.TriggerNormalEvent("MontarBestia_Bestia");
        EventsManager.TriggerNormalEvent("MontarBestia_Player");
    }

    public void AccionBestia()
    {
        EventsManager.TriggerNormalEvent("AccionBestia_Bestia");
        //EventsManager.TriggerNormalEvent("MontarBestia_Player");
    }
    public void AtaqueBestia()
    {
        EventsManager.TriggerNormalEvent("AtaqueBestia_Bestia");
        //EventsManager.TriggerNormalEvent("MontarBestia_Player");
    }

    public void ClosePanel()
    {
        beastSelectionPanel.SetActive(false);
        beastPanelEnabled = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventsManager.TriggerNormalEvent("UIPanelClosed");
    }

    public void OpenPanel()
    {
        beastSelectionPanel.SetActive(true);
        beastPanelEnabled = true;
        beast.OpenBeastMenu();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventsManager.TriggerNormalEvent("UIPanelOpened");
    }
}
