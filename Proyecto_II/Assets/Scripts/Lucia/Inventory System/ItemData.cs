using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

/* NOMBRE CLASE: Item Data
 * AUTOR: Lucía García López
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Script que se encarga de almacenar la información de un ítem.
 * VERSIÓN: 1.0 itemID, itemName, itemIcon.
 * VERSIÓN: 1.1 + itemDescription.
 */

public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
}
