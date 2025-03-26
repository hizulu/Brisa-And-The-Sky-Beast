#region Bibliotecas
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
#endregion

public class PowersDescription : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text powersBrisaDescriptionText;
    [SerializeField] private TMP_Text powersBestiaDescriptionText;
    [SerializeField] private TMP_Text powersBrisaNameText;
    [SerializeField] private TMP_Text powersBestiaNameText;

    [SerializeField] public PowersData powersData;

    // Método para detectar el clic en las imagenes
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectPowerImage();
        }
    }

    private void SelectPowerImage()
    {
        if (powersData != null)
        {
            powersBrisaDescriptionText.text = powersData.powerBrisaDescription;
            powersBrisaDescriptionText.enabled = true;
            powersBestiaDescriptionText.text = powersData.powerBestiaDescription;
            powersBestiaDescriptionText.enabled = true;
            powersBrisaNameText.text = powersData.powerBrisaName;
            powersBrisaNameText.enabled = true;
            powersBestiaNameText.text = powersData.powerBestiaName;
            powersBestiaNameText.enabled = true;
        }
    }
}
