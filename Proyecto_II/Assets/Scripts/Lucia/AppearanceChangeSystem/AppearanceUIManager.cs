using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AppearanceUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject unlockedSkinPanel;
    public GameObject blockedSkinPanel;
    public Image appearanceImage;

    [Header("UI Elements - Unlocked Panel")]
    public TMP_Text unlockedNameText;
    public TMP_Text unlockedDescriptionText;
    public Button selectSkinButton;

    [Header("UI Elements - Blocked Panel")]
    public TMP_Text blockedNameText;
    public TMP_Text blockedDescriptionText;
    public TMP_Text objectsNeededText;

    [Header("Appearance Data")]
    public List<AppearanceChangeData> appearances = new List<AppearanceChangeData>();
    private int currentAppearanceIndex = 0;

    public CharacterAppearanceManager characterAppearanceManager;

    private void Start()
    {
        if (appearances.Count > 0)
        {
            Debug.Log($"Apariencia inicial: {appearances[currentAppearanceIndex].appearanceName}");
            UpdateAppearanceUI(appearances[currentAppearanceIndex]);
        }
        else
        {
            Debug.LogError("La lista de apariencias está vacía en AppearanceUIManager.");
        }
        if (selectSkinButton != null)
        {
            selectSkinButton.onClick.AddListener(ApplyAppearanceToCharacter);
        }
    }

    public void ChangeAppearanceRight()
    {
        Debug.Log("El objeto está a punto de cambiar de padre.");
        currentAppearanceIndex++;
        if (currentAppearanceIndex >= appearances.Count)
        {
            currentAppearanceIndex = 0;
        }
        UpdateAppearanceUI(appearances[currentAppearanceIndex]);
    }

    public void ChangeAppearanceLeft()
    {
        Debug.Log("El objeto está a punto de cambiar de padre.");
        currentAppearanceIndex--;
        if (currentAppearanceIndex < 0)
        {
            currentAppearanceIndex = appearances.Count - 1;
        }
        UpdateAppearanceUI(appearances[currentAppearanceIndex]);
    }

    private void UpdateAppearanceUI(AppearanceChangeData newAppearance)
    {
        if (newAppearance == null)
        {
            Debug.LogError("Nueva apariencia es NULL.");
            return;
        }

        // Cambiar la imagen principal
        if (appearanceImage != null && newAppearance.appearanceIcon != null)
        {
            appearanceImage.sprite = newAppearance.appearanceIcon;
        }
        else
        {
            Debug.LogError("appearanceImage no está asignado o appearanceIcon es NULL.");
        }

        if (newAppearance.isUnlocked)
        {
            // Mostrar panel desbloqueado y ocultar bloqueado
            unlockedSkinPanel.SetActive(true);
            blockedSkinPanel.SetActive(false);
            selectSkinButton.interactable = true;

            // Actualizar textos
            if (unlockedNameText != null) unlockedNameText.text = newAppearance.appearanceName;
            if (unlockedDescriptionText != null) unlockedDescriptionText.text = newAppearance.appearanceDescription;
        }
        else
        {
            // Mostrar panel bloqueado y ocultar desbloqueado
            unlockedSkinPanel.SetActive(false);
            blockedSkinPanel.SetActive(true);
            selectSkinButton.interactable = false;

            // Actualizar textos
            if (blockedNameText != null) blockedNameText.text = newAppearance.appearanceName;
            if (blockedDescriptionText != null) blockedDescriptionText.text = newAppearance.appearanceDescription;
            if (objectsNeededText != null) objectsNeededText.text = $"Objetos necesarios: {newAppearance.objectsNeeded}";
        }
    }

    public void ApplyAppearanceToCharacter()
    {
        if (appearances.Count > 0 && appearances[currentAppearanceIndex].isUnlocked && characterAppearanceManager != null)
        {
            characterAppearanceManager.ChangeAppearance(appearances[currentAppearanceIndex]);
        }
    }

}
