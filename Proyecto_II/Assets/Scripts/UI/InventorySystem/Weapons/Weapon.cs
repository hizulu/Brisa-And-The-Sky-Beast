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
    #region Variables
    [SerializeField] public WeaponData weaponData; 
    private bool isCollected = false;
    #endregion

    public void CollectWeapon()
    {
        if (isCollected) return;  // Evita recogerlo más de una vez
        isCollected = true;

        gameObject.SetActive(false);  // Desactiva el objeto
        Destroy(gameObject, 0.1f);  // Lo destruye después de un pequeño delay
    }
}
