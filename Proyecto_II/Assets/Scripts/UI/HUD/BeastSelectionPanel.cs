#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
#endregion

/* NOMBRE CLASE: Beast Selection Panel
 * AUTORES: Lucía García López, Sara Yue Madruga Martín y Jone Sain Egea
 * FECHA: 03/04/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar el panel de selección de la Bestia.
 * VERSIÓN: 1.0 Lucía: Sistema de selección de acciones de la bestia inicial.
 * 
 * 1.? Lucía: Añadido el sistema de colores para los botones cuando están activos o inactivos.
 */

public class BeastSelectionPanel : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuración")]
    [SerializeField] private GameObject beastSelectionPanel;
    [SerializeField] private Beast beast;
    [SerializeField] private ItemData healingMango;
    [SerializeField] private Button petButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button rideButton;
    [SerializeField] private Button actionButton;
    [SerializeField] private Button attackButton;

    [Header("Colores")]
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private bool beastPanelEnabled = false;

    private void Awake()
    {
        if (beastSelectionPanel == null)
            beastSelectionPanel = gameObject;

        // Suscribir los eventos
        EventsManager.CallNormalEvents("BeastActionableEntered", UpdateActionButton);
        EventsManager.CallNormalEvents("BeastActionableExited", UpdateActionButton);
    }

    private void OnDestroy()
    {
        // Desuscribir los eventos
        EventsManager.StopCallNormalEvents("BeastActionableEntered", UpdateActionButton);
        EventsManager.StopCallNormalEvents("BeastActionableExited", UpdateActionButton);
    }

    private void Start()
    {
        // Inicializar colores de los botones
        ResetAllButtonColors();
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

        // Actualizar colores después de sanar
        UpdateButtonColors();
    }

    public void MontarBestia()
    {
        EventsManager.TriggerNormalEvent("MontarBestia_Bestia");
        EventsManager.TriggerNormalEvent("MontarBestia_Player");
    }

    public void AccionBestia()
    {
        EventsManager.TriggerNormalEvent("AccionBestia_Bestia");
        EventsManager.CallNormalEvents("BeastActionableZone", UpdateActionButton);
        EventsManager.CallNormalEvents("BeastActionableEntered", UpdateActionButton);
        EventsManager.CallNormalEvents("BeastActionableExited", UpdateActionButton);
        UpdateButtonColors();
    }

    public void AtaqueBestia()
    {
        EventsManager.TriggerNormalEvent("AtaqueBestia_Bestia");
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

        // Forzar actualización de todos los botones al abrir el panel
        UpdateButtonColors();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventsManager.TriggerNormalEvent("UIPanelOpened");
    }

    // Método privado para actualizar todos los colores de los botones
    private void UpdateButtonColors()
    {
        // Verificar referencias esenciales
        if (beast == null || beast.blackboard == null || InventoryManager.Instance == null)
        {
            Debug.LogError("Referencias esenciales faltantes!");
            return;
        }

        // Actualizar botón de sanar con lógica completa
        UpdateHealButton();

        // Actualizar botón de acción
        UpdateActionButton();

        // Los demás botones siempre activos
        SetButtonActive(petButton);
        SetButtonActive(rideButton);
        SetButtonActive(attackButton);
    }

    private void UpdateHealButton()
    {
        if (healButton == null || healButton.image == null) return;

        bool hasMango = InventoryManager.Instance.CheckForItem(healingMango);
        //bool isHealActive = beast.blackboard.GetValue<bool>("isOptionHeal");
        bool needsHealing = beast.currentHealth < beast.maxHealth;

        healButton.image.color = (hasMango && needsHealing) ? activeColor : inactiveColor;

        Debug.Log($"Botón Sanar - Mango: {hasMango}, Necesita cura: {needsHealing}");
    }

    private void UpdateActionButton()
    {
        if (actionButton == null || actionButton.image == null) return;

        // Obtener el estado de la zona de acción desde la bestia
        bool isInActionZone = beast != null && beast.blackboard != null &&
                             beast.blackboard.GetValue<bool>("isInActionZone");

        actionButton.image.color = isInActionZone ? activeColor : inactiveColor;
        Debug.Log($"Botón Acción - En zona: {isInActionZone}");
    }

    private void SetButtonActive(Button button)
    {
        if (button != null && button.image != null)
            button.image.color = activeColor;
    }

    private void ResetAllButtonColors()
    {
        if (petButton != null && petButton.image != null) petButton.image.color = inactiveColor;
        if (healButton != null && healButton.image != null) healButton.image.color = inactiveColor;
        if (rideButton != null && rideButton.image != null) rideButton.image.color = inactiveColor;
        if (actionButton != null && actionButton.image != null) actionButton.image.color = inactiveColor;
        if (attackButton != null && attackButton.image != null) attackButton.image.color = inactiveColor;
    }
}