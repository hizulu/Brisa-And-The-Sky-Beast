using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject inventoryMenu;
    private bool inventoryEnabled = false;
    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventoryEnabled = !inventoryEnabled;
            inventoryMenu.SetActive(inventoryEnabled);
            Time.timeScale = inventoryEnabled ? 0 : 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && inventoryEnabled)
        {
            inventoryEnabled = false;
            inventoryMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void AddItem(ItemData itemData, int quantity)
    {
        if (inventory.ContainsKey(itemData))
        {
            inventory[itemData] += quantity; // Sumar correctamente
            UpdateItemSlot(itemData, inventory[itemData]); // Actualizar UI
        }
        else
        {
            inventory[itemData] = quantity;
            AssignOrCreateItemSlot(itemData, quantity);
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

    private void AssignOrCreateItemSlot(ItemData itemData, int quantity)
    {
        // Si el primer slot está vacío, úsalo en vez de crear uno nuevo
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(itemData, quantity);
                return;
            }
        }

        // Si no hay slots vacíos, crear uno nuevo
        GameObject newSlot = Instantiate(itemSlotPrefab, inventoryPanel);
        ItemSlot slotComponent = newSlot.GetComponent<ItemSlot>();

        if (slotComponent != null)
        {
            slotComponent.SetItem(itemData, quantity);
            itemSlots.Add(slotComponent);
        }
    }

}
