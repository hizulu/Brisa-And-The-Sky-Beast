#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#endregion

/* NOMBRE CLASE: Item image and description
 * AUTOR: Lucía García López
 * FECHA: 21/03/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar la imagen y la descripción de un ítem.
 * VERSIÓN: 1.2 
 */

public class ItemImageAndDescription : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemNameText;

    private void Start()
    {
        ClearDisplay();
    }

    // Método para actualizar la imagen y la descripción de un ítem
    public void SetImageAndDescription(ItemData itemData)
    {
        if (itemData != null && itemData.itemIcon != null)
        {
            Debug.Log("Setting sprite: " + itemData.itemIcon.name); // Debug the sprite name
            itemIconImage.gameObject.SetActive(true); // Ensure the GameObject is active
            itemIconImage.sprite = itemData.itemIcon;
            itemIconImage.enabled = true; // Ensure the Image component is enabled

            itemNameText.text = itemData.itemName;
            itemDescriptionText.text = itemData.itemDescription;
            itemNameText.enabled = true;
            itemDescriptionText.enabled = true;
        }
        else
        {
            ClearDisplay();
        }
    }


    // Método para limpiar la interfaz cuando no hay ítem seleccionado
    public void ClearDisplay()
    {
        itemIconImage.enabled = false;
        itemDescriptionText.text = "";
        itemDescriptionText.enabled = false;
        itemNameText.text = "";
        itemNameText.enabled = false;
    }
}
