#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#endregion

/* NOMBRE CLASE: Oc�bjects image and description
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 21/03/2025
 * DESCRIPCI�N: Script que se encarga de gestionar la imagen y la descripci�n de un objeto.
 * VERSI�N: 1.2 
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

    // M�todo para actualizar la imagen y la descripci�n de un �tem
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

    // M�todo para actualizar la imagen y la descripci�n de un arma
    public void WeaponSetImageAndDescription(WeaponData weaponData)
    {
        if (weaponData != null && weaponData.weaponSquareIcon != null)
        {
            Debug.Log("Setting sprite: " + weaponData.weaponSquareIcon.name); // Debug para verificar el sprite
            objectIconImage.gameObject.SetActive(true); // Aseg�rate de que el GameObject est� activo
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

    // M�todo para limpiar la interfaz cuando no hay �tem seleccionado
    public void ClearDisplay()
    {
        objectIconImage.enabled = false;
        objectDescriptionText.text = "";
        objectDescriptionText.enabled = false;
        objectNameText.text = "";
        objectNameText.enabled = false;
    }
}
