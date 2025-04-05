#region Bibliotecas
using System.Collections;
using UnityEngine;
#endregion

/* NOMBRE CLASE: Weapon
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 05/04/2025
 * DESCRIPCI�N: Script que se encarga de almacenar las armas.
 * VERSI�N: 1.0 
 */

public class Weapon : MonoBehaviour
{
    #region Variables
    [SerializeField] public WeaponData weaponData; 
    private bool isCollected = false;
    #endregion

    public void CollectWeapon()
    {
        if (isCollected) return;  // Evita recogerlo m�s de una vez
        isCollected = true;

        gameObject.SetActive(false);  // Desactiva el objeto
        Destroy(gameObject, 0.1f);  // Lo destruye despu�s de un peque�o delay
    }
}
