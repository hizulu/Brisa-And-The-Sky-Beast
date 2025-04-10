#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
#endregion

/* NOMBRE CLASE: Beast Selection Panel
 * AUTOR: Lucía García López
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar el panel de selección de la Bestia.
 * VERSIÓN: 1.0
 */

public class BeastSelectionPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject beastSelectionPanel;
    private bool inventoryEnabled = false;

    private void Awake()
    {
        if (beastSelectionPanel == null)
            beastSelectionPanel = gameObject;
    }

    // Método para abrir y cerrar el panel de selección de bestias
    public void OpenCloseBeastPanel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.name == "tab")
        {
            if (inventoryEnabled)
            {
                beastSelectionPanel.SetActive(false);
                inventoryEnabled = false;
            }
            else
            {
                beastSelectionPanel.SetActive(true);
                inventoryEnabled = true;
            }
            Time.timeScale = inventoryEnabled ? 0f : 1f;
        }
    }

    //TODO Esto tengo que revisarlo
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Panel clicked!");
    }

    public void AcariciarBestia()
    {
        EventsManager.TriggerNormalEvent("AcariciarBestia");
    }
}
