using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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

    private PowersLocked powersLocked;

    private void Start()
    {
        powersLocked = GetComponentInChildren<PowersLocked>(); // Asegúrate de que PowersLocked esté en un hijo de este objeto
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

        // Cambiamos las fuentes de ambos nombres según el estado
        powersLocked.UpdatePowerNamesFont(isUnlocked);

        if (!isUnlocked)
        {
            LockedText();
        }
        else
        {
            PowerDescriptionText();
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
}