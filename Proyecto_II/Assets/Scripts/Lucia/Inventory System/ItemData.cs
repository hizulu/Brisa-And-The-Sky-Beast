using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

/* NOMBRE CLASE: Item Data
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 13/03/2025
 * DESCRIPCI�N: Script que se encarga de almacenar la informaci�n de un �tem.
 * VERSI�N: 1.0 appearanceID, appearanceName, appearanceIcon.
 * VERSI�N: 1.1 + appearanceDescription.
 */

public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
}
