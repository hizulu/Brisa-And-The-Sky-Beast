using UnityEngine;
using UnityEngine.InputSystem;

/*
 * NOMBRE CLASE: MapManager
 * AUTOR: Lucía García López
 * FECHA: 19/04/2025
 * DESCRIPCIÓN: Script que gestiona el mapa del juego. Permite abrir y cerrar el panel del mapa.
 * VERSIÓN: 1.0 Sistema de mapa inicial.
 */

public class MapManager : MonoBehaviour
{
    #region Variables
    [Header("Map Configuration")]
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private Camera mapCamera;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput playerInput;
    #endregion

    private void Awake()
    {
        if (mapPanel == null)
        {
            Debug.LogError("mapPanel no asignado en el Inspector!");
            return;
        }
    }

    //Se activa el panel del mapa al presionar la tecla "M" y se desactiva al volver a presionarla.
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

    //Método para cerrar el panel del mapa.
    public void ClosePanel()
    {
        mapPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventsManager.TriggerNormalEvent("UIPanelClosed");
        Debug.Log("Map closed");
    }

    //Método para abrir el panel del mapa.
    private void OpenPanel()
    {
        mapPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventsManager.TriggerNormalEvent("UIPanelOpened");
        Debug.Log("Map opened");
    }
}