#region Bibliotecas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Item Checker
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 30/03/2025
 * DESCRIPCI�N: Script que se encarga de comprobar si un �tem est� en el inventario.
 * VERSI�N: 1.0 
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
            Debug.Log("�El inventario tiene el �tem?: " + hasItem);
        }
        else
        {
            Debug.LogWarning("InventoryManager no est� inicializado.");
        }
    }
}
