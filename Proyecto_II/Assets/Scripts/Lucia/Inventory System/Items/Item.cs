#region Bibliotecas
using System.Collections;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Item
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de almacenar los items recogidos en el inventario.
 * VERSI�N: 1.0 
 */

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData; // Necesito ponerlo en p�blico para acceder a �l desde el inventario y comprobar.
    [SerializeField] private int itemQuantity = 1;
    private bool isCollected = false;

    public void CollectItem()
    {
        if (isCollected) return;  // Evita recogerlo m�s de una vez
        isCollected = true;

        InventoryManager.Instance.AddItem(itemData, itemQuantity);
        gameObject.SetActive(false);  // Desactiva el objeto
        Destroy(gameObject, 1f);  // Lo destruye despu�s de un peque�o delay
    }
}
