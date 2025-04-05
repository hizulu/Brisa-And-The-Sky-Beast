#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#endregion

/* NOMBRE CLASE: Oc¡bjects image and description
 * AUTOR: Lucía García López
 * FECHA: 21/03/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar la imagen y la descripción de un objeto.
 * VERSIÓN: 1.2 
 */


public class ObjectsImageAndDescription : MonoBehaviour
{
    [SerializeField] private Image objectIconImage;
    [SerializeField] private TMP_Text objectDescriptionText;
    [SerializeField] private TMP_Text objectNameText;

    private void Start()
    {
        ClearDisplay();
    }

    // Método para actualizar la imagen y la descripción de un ítem
    public void ItemSetImageAndDescription(ItemData itemData)
    {
        if (itemData != null && itemData.itemIcon != null)
        {
            Debug.Log("Setting sprite: " + itemData.itemIcon.name); // Debug the sprite name
            objectIconImage.gameObject.SetActive(true); // Ensure the GameObject is active
            objectIconImage.sprite = itemData.itemIcon;
            objectIconImage.enabled = true; // Ensure the Image component is enabled

            objectNameText.text = itemData.itemName;
            objectDescriptionText.text = itemData.itemDescription;
            objectNameText.enabled = true;
            objectDescriptionText.enabled = true;
        }
        else
        {
            ClearDisplay();
        }
    }

    // Método para actualizar la imagen y la descripción de un arma
    public void WeaponSetImageAndDescription(WeaponData weaponData)
    {
        if (weaponData != null && weaponData.weaponSquareIcon != null)
        {
            Debug.Log("Setting sprite: " + weaponData.weaponSquareIcon.name); // Debug para verificar el sprite
            objectIconImage.gameObject.SetActive(true); // Asegúrate de que el GameObject está activo
            objectIconImage.sprite = weaponData.weaponSquareIcon;
            objectIconImage.enabled = true; // Habilitar el componente Image

            objectNameText.text = weaponData.weaponName;
            objectDescriptionText.text = weaponData.weaponDescription;
            objectNameText.enabled = true;
            objectDescriptionText.enabled = true;
        }
        else
        {
            ClearDisplay(); // Si no hay arma, limpia la UI
        }
    }

    // Método para limpiar la interfaz cuando no hay ítem seleccionado
    public void ClearDisplay()
    {
        objectIconImage.enabled = false;
        objectDescriptionText.text = "";
        objectDescriptionText.enabled = false;
        objectNameText.text = "";
        objectNameText.enabled = false;
    }
}
