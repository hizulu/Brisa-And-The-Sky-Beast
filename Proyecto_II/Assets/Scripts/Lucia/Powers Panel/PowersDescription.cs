using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PowersDescription : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text powersBrisaDescriptionText;
    [SerializeField] private TMP_Text powersBestiaDescriptionText;
    [SerializeField] private TMP_Text powersBrisaNameText;
    [SerializeField] private TMP_Text powersBestiaNameText;
    [SerializeField] private TMP_Text whereToFindText;

    [SerializeField] private GameObject powersPanelLocked;

    [SerializeField] public PowersData powersData;
    private PowersLocked powersLocked;

    private void Start()
    {
        powersLocked = GetComponentInChildren<PowersLocked>(); // Asegúrate de que PowersLocked esté en un hijo de este objeto
        if (powersData != null)
        {
            powersPanelLocked.SetActive(!powersData.isUnlocked);
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
            if (powersData.isUnlocked)
            {
                powersPanelLocked.SetActive(false);
                powersLocked.ChangeFont(true); // Desbloqueado, cambiar a la fuente de Santana
            }
            else
            {
                powersPanelLocked.SetActive(true);
                powersLocked.ChangeFont(false); // Bloqueado, cambiar a la fuente de Fenara
                LockedText();
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
    }
}
