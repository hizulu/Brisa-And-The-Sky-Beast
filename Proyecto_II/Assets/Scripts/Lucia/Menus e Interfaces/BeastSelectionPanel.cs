using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BeastSelectionPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject beastSelectionPanel;
    private bool inventoryEnabled = false;

    private void Awake()
    {
        if (beastSelectionPanel == null)
            beastSelectionPanel = gameObject;
    }

    // Función para abrir y cerrar el panel con la tecla 'Tab'
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

    //Esto tengo que revisarlo
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Panel clicked!");
    }
}
