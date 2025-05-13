using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]

/* NOMBRE CLASE: Weapon Data
 * AUTOR: Lucía García López
 * FECHA: 05/04/2025
 * DESCRIPCIÓN: Script que se encarga de almacenar la información de un arma.
 * VERSIÓN: 1.0 weaponID, weaponName, weaponVerticalIcon, weaponSquareIcon, weaponDescription.
 */

public class WeaponData : ScriptableObject
{
    public string weaponID;
    public string weaponName;
    public Sprite weaponVerticalIcon;
    public Sprite weaponSquareIcon;
    public string weaponDescription;
}
