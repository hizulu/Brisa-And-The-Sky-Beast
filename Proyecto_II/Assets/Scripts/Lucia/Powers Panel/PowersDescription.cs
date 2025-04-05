#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#endregion

/* NOMBRE CLASE: Powers Description
 * AUTOR: Lucía García López
 * FECHA: 31/03/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar la descripción de los poderes en el panel de poderes.
 * VERSIÓN: 1.0
 * 1.1 LockedImage, UnlockedImage.
 */

public class PowersDescription : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    [SerializeField] private TMP_Text powersBrisaDescriptionText;
    [SerializeField] private TMP_Text powersBestiaDescriptionText;
    [SerializeField] private TMP_Text powersBrisaNameText;
    [SerializeField] private TMP_Text powersBestiaNameText;
    [SerializeField] private TMP_Text powersBrisaLockedNameText;
    [SerializeField] private TMP_Text powersBestiaLockedNameText;
    [SerializeField] private TMP_Text whereToFindText;
    [SerializeField] private GameObject powersPanelLocked;
    [SerializeField] private GameObject powersPanelUnlocked;
    [SerializeField] public PowersData powersData;
    private Image image;
    #endregion

    private void Start()
    {
        image = GetComponent<Image>();

        UpdatePanelState();
    }

    // Método para poder hacer clic en la imagen
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectPowerImage();
        }
    }

    //Método para seleccionar la imagen del poder
    private void SelectPowerImage()
    {
        if (powersData != null)
        {
            UpdatePanelState();
            PowerDescriptionText();
        }
    }

    //Método para actualizar el estado del panel
    private void UpdatePanelState()
    {
        bool isUnlocked = powersData.isUnlocked;

        powersPanelLocked.SetActive(!isUnlocked);
        powersPanelUnlocked.SetActive(isUnlocked);

        if (!isUnlocked)
        {
            LockedText();
            LockedImage();
        }
        else
        {
            PowerDescriptionText();
            UnlockedImage();
        }
    }

    //Método para mostrar el texto de la descripción del poder
    private void LockedText()
    {
        whereToFindText.text = powersData.whereToFind;
        powersBrisaLockedNameText.text = powersData.powerBrisaName;
        powersBestiaLockedNameText.text = powersData.powerBestiaName;
    }

    //Método para mostrar el texto de la descripción del poder
    private void PowerDescriptionText()
    {
        powersBrisaDescriptionText.text = powersData.powerBrisaDescription;
        powersBestiaDescriptionText.text = powersData.powerBestiaDescription;
        powersBrisaNameText.text = powersData.powerBrisaName;
        powersBestiaNameText.text = powersData.powerBestiaName;
    }

    //Método para cambiar el color de la imagen bloqueada a gris
    private void LockedImage()
    {
        //Cambia el color de la imagen a un gris claro con opacidad
        image.color = new Color(0.75f, 0.75f, 0.75f, 1f);
    }

    //Método para cambiar el color de la imagen desbloqueada a su color normal
    private void UnlockedImage()
    {
        //Cambia el color a blanco
        image.color = new Color(1f, 1f, 1f, 1f);
    }
}
