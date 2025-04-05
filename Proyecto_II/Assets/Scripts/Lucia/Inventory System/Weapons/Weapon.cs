#region Bibliotecas
using System.Collections;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Weapon
 * AUTOR: Lucía García López
 * FECHA: 05/04/2025
 * DESCRIPCIÓN: Script que se encarga de almacenar las armas.
 * VERSIÓN: 1.0 
 */

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponData weaponData; 
    private bool isCollected = false;

    public void CollectWeapon()
    {
        if (isCollected) return;  // Evita recogerlo más de una vez
        isCollected = true;

        //InventoryManager.Instance.AddItem(itemData, itemQuantity);
        gameObject.SetActive(false);  // Desactiva el objeto
        Destroy(gameObject, 0.1f);  // Lo destruye después de un pequeño delay
    }
}
