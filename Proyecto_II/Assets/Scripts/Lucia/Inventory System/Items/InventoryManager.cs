#region Bibliotecas
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
#endregion

/* NOMBRE CLASE: Inventory Manager
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de gestionar el inventario del jugador.
 * VERSI�N: 1.0 
 * 1.1 AppearanceChangeMenu, appearanceChangeEnabled, mapMenu, mapEnabled.
 * 1.2 powersMenu, powersEnabled.
 * 1.3 CheckForItem.
 * 1.4 RemoveItem, UpdateItemQuantity, UpdateItemSlotVisibility OnValidate.
 * 1.5 Inventory Open and Close with i key.
 */

public class InventoryManager : MonoBehaviour
{
    #region Variables
    [Header("Paneles")]
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

    [Header("Apariencia")]
    public List<AppearanceChangeData> appearanceData; // Referencia a la apariencia que se va a desbloquear
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
    // M�todo llamado cuando un valor cambia en el Inspector
    //private void OnValidate()
    //{
    //    // Verifica todos los slots en el inventario
    //    foreach (var slot in itemSlots)
    //    {
    //        if (slot != null && slot.HasItem())
    //        {
    //            slot.UpdateQuantity(inventory[slot.GetItemData()]); // Actualiza la cantidad de cada �tem
    //            slot.OnValidate(); // Llama al OnValidate de ItemSlot
    //        }
    //    }
    //}

    public void AddItem(ItemData itemData, int quantity)
    {
        // 1. Actualizar inventario
        if (inventory.ContainsKey(itemData))
        {
            inventory[itemData] += quantity;
            UpdateItemSlotVisibility(itemData);
        }
        else
        {
            inventory.Add(itemData, quantity);
            AssignOrCreateItemSlot(itemData, quantity);
        }

        // 2. Verificar desbloqueos
        bool anyAppearanceUnlocked = false;
        foreach (var appearance in appearanceData)
        {
            if (appearance.objectsNeededPrefab == itemData)
            {
                bool wasUnlocked = appearance.isUnlocked;
                bool unlockedNow = AppearanceUnlock.Instance.TryUnlockAppearance(appearance);

                if (unlockedNow)
                {
                    Debug.Log($"�Apariencia {appearance.appearanceName} desbloqueada!");
                    anyAppearanceUnlocked = true;
                }

                // Actualizar UI siempre que sea el �tem requerido
                AppearanceUIManager.Instance.UpdateAppearanceUI(appearance);
            }
        }

        // 3. Notificaciones
        if (anyAppearanceUnlocked)
        {
            EventsManager.TriggerNormalEvent("NewAppearanceUnlocked");
        }
        EventsManager.TriggerSpecialEvent("InventoryUpdated", itemData);
    }

    //M�todo para eliminar un �tem del inventario
    public void RemoveItem(ItemData itemData)
    {
        if (inventory.ContainsKey(itemData)) // Verifica si el �tem existe en el inventario
        {
            int quantity = inventory[itemData];
            quantity = quantity - 1;
            UpdateItemQuantity(itemData, quantity); //TODO Revisar si es necesario o si funciona bien

            UpdateItemSlotVisibility(itemData);
        }
    }

    // M�todo para actualizar la cantidad de un �tem en un slot
    public void UpdateItemQuantity(ItemData itemData, int newQuantity)
    {
        if (inventory.ContainsKey(itemData)) // Verifica si el �tem existe en el inventario
        {
            inventory[itemData] = newQuantity; // Actualiza la cantidad
        }
    }

    // M�todo para actualizar la visibilidad de un slot
    public void UpdateItemSlotVisibility(ItemData itemData)
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.GetItemData() == itemData) // Si el slot contiene el �tem
            {
                int quantity = inventory[itemData]; // Obtiene la cantidad del �tem

                // Si la cantidad es 0 o menor, desactiva el slot
                if (quantity <= 0)
                {
                    slot.gameObject.SetActive(false); // Desactiva el slot
                    slot.DeselectItem(); // Desactiva la selecci�n del slot
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

    //Asignar o crear un nuevo slot para un �tem
    private void AssignOrCreateItemSlot(ItemData itemData, int quantity)
    {
        // Buscar un slot vac�o y asignarle el �tem
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(itemData, quantity);
                slot.gameObject.SetActive(true);
                return;
            }
        }

        // Si no hay slots vac�os, crear uno nuevo y activarlo
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

    public int GetItemQuantity(ItemData itemData)
    {
        if (inventory.TryGetValue(itemData, out int quantity)) // Verifica si el �tem existe en el inventario
        {
            return quantity; // Devuelve la cantidad del �tem
        }
        return 0; // Si no existe, devuelve 0
    }

    // M�todo para comprobar por el nombre de un item.
    public ItemData GetItemByName(string itemName)
    {
        foreach (ItemData item in inventory.Keys) // Recorremos el diccionario
        {
            if (item.itemName == itemName) // Comprobamos si est� el objeto con el nombre escpec�fico que queremos.
                return item; // Si est�, nos devuelve el item espec�fico.
        }

        return null; // Si no est�, nos devuelve un nulo.
    }

    //Abrir y cerrar inventario usando el Input System
    public void OpenCloseInventory(InputAction.CallbackContext context)
    {
        string keyPressed = context.control.name;

        if (!context.performed)
            return;

        if (keyPressed == "i" && firstTime)
        {
            inventoryEnabled = true;
            firstTime = false;
            EventsManager.TriggerNormalEvent("UIPanelOpened");
        }
        else if ((keyPressed == "i") && !firstTime)
        {
            inventoryEnabled = false;
            appearanceChangeEnabled = false;
            mapEnabled = false;
            powersEnabled = false;
            firstTime = true;

            EventsManager.TriggerNormalEvent("UIPanelClosed");
            DeselectAllItems();        }

        inventoryMenu.SetActive(inventoryEnabled);
        AppearanceChangeMenu.SetActive(appearanceChangeEnabled);
        mapMenu.SetActive(mapEnabled);
        powersMenu.SetActive(powersEnabled);
        Time.timeScale = inventoryEnabled ? 0 : 1;
        Cursor.visible = inventoryEnabled;
        Cursor.lockState = inventoryEnabled ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CloseInventory()
    {
        inventoryEnabled = false;
        appearanceChangeEnabled = false;
        mapEnabled = false;
        powersEnabled = false;
        firstTime = true;

        EventsManager.TriggerNormalEvent("UIPanelClosed");
        DeselectAllItems();

        inventoryMenu.SetActive(inventoryEnabled);
        AppearanceChangeMenu.SetActive(appearanceChangeEnabled);
        mapMenu.SetActive(mapEnabled);
        powersMenu.SetActive(powersEnabled);
        Time.timeScale = inventoryEnabled ? 0 : 1;
        Cursor.visible = inventoryEnabled;
        Cursor.lockState = inventoryEnabled ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // M�todo para deseleccionar todos los �tems cuando se cierra el inventario
    private void DeselectAllItems()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.DeselectItem(); // Desactiva la selecci�n de cada slot
        }
    }
}
