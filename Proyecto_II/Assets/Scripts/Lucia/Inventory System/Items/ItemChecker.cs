#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Item Checker
 * AUTOR: Lucía García López
 * FECHA: 30/03/2025
 * DESCRIPCIÓN: Script que se encarga de comprobar si un ítem está en el inventario.
 * VERSIÓN: 1.0 
 */

public class ItemChecker : MonoBehaviour
{
    private InventoryManager inventoryManager;
    [SerializeField] private ItemData specialItemData;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
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
