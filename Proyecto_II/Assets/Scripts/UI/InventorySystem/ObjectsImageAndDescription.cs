#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#endregion

/* NOMBRE CLASE: Objects image and description
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 21/03/2025
 * DESCRIPCI�N: Script que se encarga de gestionar la imagen y la descripci�n de un objeto.
 * VERSI�N: 1.3 WeaponSetImageAndDescription.
 */


public class ObjectsImageAndDescription : MonoBehaviour
{
    [SerializeField] private Image objectIconImage;
    [SerializeField] private TMP_Text objectDescriptionText;
    [SerializeField] private TMP_Text objectNameText;

    #region Singleton
    public static ObjectsImageAndDescription Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        ClearDisplay();
    }

    // M�todo para actualizar la imagen y la descripci�n de un �tem
    public void ItemSetImageAndDescription(ItemData itemData)
    {
        if (itemData != null && itemData.itemIcon != null)
        {
            Debug.Log("Setting sprite: " + itemData.itemIcon.name);
            objectIconImage.gameObject.SetActive(true);
            objectIconImage.sprite = itemData.itemIcon;
            objectIconImage.enabled = true;

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
            Debug.Log("Setting sprite: " + weaponData.weaponSquareIcon.name);
            objectIconImage.gameObject.SetActive(true);
            objectIconImage.sprite = weaponData.weaponSquareIcon;
            objectIconImage.enabled = true;

            objectNameText.text = weaponData.weaponName;
            objectDescriptionText.text = weaponData.weaponDescription;
            objectNameText.enabled = true;
            objectDescriptionText.enabled = true;
        }
        else
        {
            ClearDisplay();
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
