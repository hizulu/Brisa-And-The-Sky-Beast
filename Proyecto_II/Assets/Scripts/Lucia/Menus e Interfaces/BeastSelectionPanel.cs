using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BeastSelectionPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject beastSelectionPanel; // Referencia al panel
    private bool inventoryEnabled = false; // Estado del panel (abierto o cerrado)

    private void Awake()
    {
        // Si beastSelectionPanel no est� asignado, intenta obtenerlo autom�ticamente.
        if (beastSelectionPanel == null)
            beastSelectionPanel = gameObject; // Asumiendo que el script est� en el objeto del panel.
    }

    // Funci�n para abrir y cerrar el panel con la tecla 'Tab'
    public void OpenCloseBeastPanel(InputAction.CallbackContext context)
    {
        // Verifica si el contexto se ha activado correctamente
        if (!context.performed)
            return;

        // Verifica si la tecla 'Tab' fue presionada
        if (context.control.name == "tab")
        {
            if (inventoryEnabled)
            {
                beastSelectionPanel.SetActive(false); // Desactiva el panel
                inventoryEnabled = false;
            }
            else
            {
                beastSelectionPanel.SetActive(true); // Activa el panel
                inventoryEnabled = true;
            }

            // Opcional: Pausar el juego cuando el panel est� abierto
            Time.timeScale = inventoryEnabled ? 0f : 1f;
        }
    }

    // Maneja los clics en el panel (se dispara si se hace clic en el panel)
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Panel clicked!"); // Imprime en consola cuando el panel es clickeado
    }
}
