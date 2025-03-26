#region Bibliotecas
using System.Collections;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Item
 * AUTOR: Lucía García López
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Script que se encarga de almacenar los itemas recogidos en el inventario.
 * VERSIÓN: 1.0 
 */

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int itemQuantity = 1;
    private bool isCollected = false;

    public void CollectItem()
    {
        if (isCollected) return;  // Evita recogerlo más de una vez
        isCollected = true;

        InventoryManager.Instance.AddItem(itemData, itemQuantity);
        gameObject.SetActive(false);  // Desactiva el objeto
        Destroy(gameObject, 0.1f);  // Lo destruye después de un pequeño delay
    }
}
