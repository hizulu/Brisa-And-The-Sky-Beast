using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowersDescription : MonoBehaviour, IPointerClickHandler
{
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

    private void Start()
    {
        image = GetComponent<Image>();

        UpdatePanelState();
    }

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
            UpdatePanelState();
            PowerDescriptionText();
        }
    }

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

    private void LockedText()
    {
        whereToFindText.text = powersData.whereToFind;
        powersBrisaLockedNameText.text = powersData.powerBrisaName;
        powersBestiaLockedNameText.text = powersData.powerBestiaName;
    }

    private void PowerDescriptionText()
    {
        powersBrisaDescriptionText.text = powersData.powerBrisaDescription;
        powersBestiaDescriptionText.text = powersData.powerBestiaDescription;
        powersBrisaNameText.text = powersData.powerBrisaName;
        powersBestiaNameText.text = powersData.powerBestiaName;
    }

    private void LockedImage()
    {
        //Cambia el color de la imagen a un gris claro con opacidad
        image.color = new Color(0.75f, 0.75f, 0.75f, 1f);
    }

    private void UnlockedImage()
    {
        //Cambia el color a blanco
        image.color = new Color(1f, 1f, 1f, 1f);
    }
}
