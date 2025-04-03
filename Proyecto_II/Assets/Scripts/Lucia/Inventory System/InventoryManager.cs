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
 * 1.2 powersMenu, powersEnabled.
 * 1.3 CheckForItem.
 * 1.4 RemoveItem, UpdateItemQuantity, UpdateItemSlotVisibility OnValidate.
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
    public GameObject powersMenu;
    public bool powersEnabled = false;

    public bool firstTime = true;

    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;
    #endregion

    #region Instancia Singleton
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
    #endregion

    //Solo para pruebas
    // Método llamado cuando un valor cambia en el Inspector
    private void OnValidate()
    {
        // Verifica todos los slots en el inventario
        foreach (var slot in itemSlots)
        {
            if (slot != null && slot.HasItem())
            {
                slot.UpdateQuantity(inventory[slot.GetItemData()]); // Actualiza la cantidad de cada ítem
                slot.OnValidate(); // Llama al OnValidate de ItemSlot
            }
        }
    }

    public void AddItem(ItemData itemData, int quantity)
    {
        if (inventory.ContainsKey(itemData))
        {
            inventory[itemData] += quantity; // Sumar al diccionario
            UpdateItemSlotVisibility(itemData); // Actualizar visibilidad del slot
        }
        else
        {
            inventory[itemData] = quantity; // Si no existe, lo añadimos
            AssignOrCreateItemSlot(itemData, quantity); // Creamos un nuevo slot
        }
    }


    public void RemoveItem(ItemData itemData)
    {
        if (inventory.ContainsKey(itemData)) // Verifica si el ítem existe en el inventario
        {
            // Actualiza la cantidad a 0 
            UpdateItemQuantity(itemData, 0); //TODO Revisar si es necesario o si es 0

            // Luego, actualiza la visibilidad del slot
            UpdateItemSlotVisibility(itemData);
        }
    }

    // Método para actualizar la cantidad de un ítem en un slot
    public void UpdateItemQuantity(ItemData itemData, int newQuantity)
    {
        if (inventory.ContainsKey(itemData)) // Verifica si el ítem existe en el inventario
        {
            inventory[itemData] = newQuantity; // Actualiza la cantidad
        }
    }

    // Método para actualizar la visibilidad de un slot
    public void UpdateItemSlotVisibility(ItemData itemData)
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.GetItemData() == itemData) // Si el slot contiene el ítem
            {
                int quantity = inventory[itemData]; // Obtiene la cantidad del ítem

                // Si la cantidad es 0 o menor, desactiva el slot
                if (quantity <= 0)
                {
                    slot.gameObject.SetActive(false); // Desactiva el slot
                }
                else
                {
                    slot.gameObject.SetActive(true); // Reactiva el slot si la cantidad es mayor que 0
                    slot.UpdateQuantity(quantity); // Actualiza la cantidad en el slot
                }

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

    //Revisar si hay un item concreto en el inventario
    public bool CheckForItem (ItemData specialItem)
    {
        if(inventory.TryGetValue(specialItem, out int quantity)&& quantity>0)
        {
            Debug.Log("Hay "+ specialItem.itemName+ " en el inventario");
            return true;
        }
        else
        {
            Debug.Log("No hay " + specialItem.itemName + " en el inventario");
            return false;
        }
    }

    //Abrir y cerrar inventario usando el Input System
    public void OpenCloseInventory(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.name == "i" && firstTime)
        {
            inventoryEnabled = true;
            firstTime = false;
        }
        else if (context.control.name == "i" && !firstTime)
        {
            inventoryEnabled = false;
            appearanceChangeEnabled = false;
            mapEnabled = false;
            powersEnabled = false;
            firstTime = true;
        }

        if (inventoryEnabled && context.control.name == "escape") //TODO esto no funciona
        {
            inventoryEnabled = false;
            appearanceChangeEnabled = false;
            mapEnabled = false;
            powersEnabled = false;
            firstTime = true;
        }

        inventoryMenu.SetActive(inventoryEnabled);
        AppearanceChangeMenu.SetActive(appearanceChangeEnabled);
        mapMenu.SetActive(mapEnabled);
        powersMenu.SetActive(powersEnabled);
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
