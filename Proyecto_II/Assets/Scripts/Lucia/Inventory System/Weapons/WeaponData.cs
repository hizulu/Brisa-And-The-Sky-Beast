using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]

/* NOMBRE CLASE: Weapon Data
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 05/04/2025
 * DESCRIPCI�N: Script que se encarga de almacenar la informaci�n de un arma.
 * VERSI�N: 1.0 weaponID, weaponName, weaponVerticalIcon, weaponSquareIcon, weaponDescription.
 */

public class WeaponData : ScriptableObject
{
    public string weaponID;
    public string weaponName;
    public Sprite weaponVerticalIcon;
    public Sprite weaponSquareIcon;
    public string weaponDescription;
}
