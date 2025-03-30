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
        if (powersData != null)
        {
            powersPanelLocked.SetActive(!powersData.isUnlocked);
            powersPanelUnlocked.SetActive(powersData.isUnlocked);
        }
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
            bool isUnlocked = powersData.isUnlocked;

            powersPanelLocked.SetActive(true);  // Primero activa para asegurarte de que cambia de estado
            powersPanelUnlocked.SetActive(false);

            powersPanelLocked.SetActive(!isUnlocked);
            powersPanelUnlocked.SetActive(isUnlocked);

            Debug.Log("Locked Panel: " + powersPanelLocked.activeSelf);
            Debug.Log("Unlocked Panel: " + powersPanelUnlocked.activeSelf);

            powersLocked.ChangeFont(isUnlocked);

            if (!isUnlocked)
            {
                powersPanelUnlocked.SetActive(false);
                powersPanelLocked.SetActive(true);
                LockedText();
            }
            else if (isUnlocked)
            {
                powersPanelLocked.SetActive(false);
                powersPanelUnlocked.SetActive(true);
            }

            PowerDescriptionText();
        }
    }

    private void PowerDescriptionText()
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

    private void LockedText()
    {
        whereToFindText.text = powersData.whereToFind;
        whereToFindText.enabled = true;
        powersBrisaLockedNameText.text = powersData.powerBrisaName;
        powersBestiaLockedNameText.text = powersData.powerBestiaName;
    }
}
