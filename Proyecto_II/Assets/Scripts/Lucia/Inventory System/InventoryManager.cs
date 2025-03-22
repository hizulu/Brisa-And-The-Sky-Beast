#region Bibliotecas
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Inventory Manager
 * AUTOR: Lucía García López
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar el inventario del jugador.
 * VERSIÓN: 1.0 
 * 1.1 AppearanceChangeMenu, appearanceChangeEnabled, mapMenu, mapEnabled.
 */

public class InventoryManager : MonoBehaviour
{
    #region Variables
    public GameObject inventoryMenu;
    public bool inventoryEnabled = false;
    public GameObject AppearanceChangeMenu;
    public bool appearanceChangeEnabled = false;
    public GameObject mapMenu;
    public bool mapEnabled = false;
    public bool firstTime = true;

    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;
    #endregion

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Añadir un ítem al inventario usando un ItemData y una cantidad, si ya existe, sumar la cantidad
    public void AddItem(ItemData itemData, int quantity)
    {
        if (inventory.ContainsKey(itemData)) //Inventory es el diccionario
        {
            inventory[itemData] += quantity; //Suma la cantidad al ítem existente
            UpdateItemSlot(itemData, inventory[itemData]); //Actualiza la cantidad en el slot
        }
        else
        {
            inventory[itemData] = quantity;
            AssignOrCreateItemSlot(itemData, quantity); //Crea un nuevo slot si no existe
        }
    }

    private void UpdateItemSlot(ItemData itemData, int newQuantity)
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.GetItemData() == itemData) // Si ya existe el ítem en un slot
            {
                slot.UpdateQuantity(newQuantity); // Actualizar la cantidad en la UI
                return;
            }
        }
    }

    //Asignar o crear un nuevo slot para un ítem
    private void AssignOrCreateItemSlot(ItemData itemData, int quantity)
    {
        // Buscar un slot vacío y asignarle el ítem
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(itemData, quantity);
                slot.gameObject.SetActive(true);
                return;
            }
        }

        // Si no hay slots vacíos, crear uno nuevo y activarlo
        GameObject newSlot = Instantiate(itemSlotPrefab, inventoryPanel);
        newSlot.SetActive(true);

        ItemSlot slotComponent = newSlot.GetComponent<ItemSlot>();
        if (slotComponent != null)
        {
            slotComponent.SetItem(itemData, quantity);
            itemSlots.Add(slotComponent);
        }
    }

    //Abrir y cerrar inventario usando el Input System
    public void OpenCloseInventory(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.name == "f" && firstTime)
        {
            inventoryEnabled = true;
            firstTime = false;
        }
        else if (context.control.name == "f" && !firstTime)
        {
            inventoryEnabled = false;
            appearanceChangeEnabled = false;
            mapEnabled = false;
            firstTime = true;
        }

        if (inventoryEnabled && context.control.name == "escape") //TODO esto no funciona
        {
            inventoryEnabled = false;
            appearanceChangeEnabled = false;
            mapEnabled = false;
            firstTime = true;
        }

        inventoryMenu.SetActive(inventoryEnabled);
        AppearanceChangeMenu.SetActive(appearanceChangeEnabled);
        mapMenu.SetActive(mapEnabled);
        Time.timeScale = inventoryEnabled ? 0 : 1;
        Cursor.visible = inventoryEnabled ? true : false; // Hace visible el cursor
        Cursor.lockState = inventoryEnabled ? CursorLockMode.None : CursorLockMode.Locked; // Bloquea el cursor
    }

    #region MÉTODOS ANTIGUOS MODIFICADOS

    //public void OpenCloseInventory()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        inventoryEnabled = !inventoryEnabled;
    //        inventoryMenu.SetActive(inventoryEnabled);
    //        Time.timeScale = inventoryEnabled ? 0 : 1;
    //    }        
    //}
    //public void CloseInventory()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape) && inventoryEnabled)
    //    {
    //        inventoryEnabled = false;
    //        inventoryMenu.SetActive(false);
    //        Time.timeScale = 1;
    //    }
    //}

    #endregion
}
