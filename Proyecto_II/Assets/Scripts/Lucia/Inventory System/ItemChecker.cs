using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    private InventoryManager inventoryManager;
    [SerializeField] private ItemData specialItemData;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance; // Acceder a la instancia Singleton
    }

    public void CheckItemOnInventary()
    {
        if (inventoryManager != null)
        {
            bool hasItem = inventoryManager.CheckForItem (specialItemData);
            Debug.Log("¿El inventario tiene el ítem?: " + hasItem);
        }
        else
        {
            Debug.LogWarning("InventoryManager no está inicializado.");
        }
    }

}
