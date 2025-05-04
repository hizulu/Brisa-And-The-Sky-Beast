#region Bibliotecas
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static Cinemachine.CinemachinePathBase;
#endregion

/* NOMBRE CLASE: Appearance UI Manager
 * AUTOR: Lucía García López
 * FECHA: 25/03/2025
 * DESCRIPCIÓN: Script que se encarga de gestionar la interfaz de usuario para cambiar la apariencia del personaje.
 * VERSIÓN: 1.0
 */

public class AppearanceUIManager : MonoBehaviour
{
    #region Variables
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
    [SerializeField] TMP_Text objectsObtainedQuantityText;

    [Header("Appearance Data")]
    public List<AppearanceChangeData> appearances = new List<AppearanceChangeData>();
    private int currentAppearanceIndex = 0;

    public CharacterAppearanceManager characterAppearanceManager;
    #endregion

    #region Singleton
    public static AppearanceUIManager Instance { get; private set; }

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

    //Método para cambiar la apariencia a la derecha
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

    //Método para cambiar la apariencia a la izquierda
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

    //Método para cambiar la apariencia al hacer clic en el botón
    public void UpdateAppearanceUI(AppearanceChangeData newAppearance)
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
            objectsObtainedQuantityText.text = InventoryManager.Instance.GetItemQuantity(newAppearance.objectsNeededPrefab).ToString();
        }
    }

    //Método para aplicar la apariencia al personaje
    public void ApplyAppearanceToCharacter()
    {
        if (appearances.Count > 0 && characterAppearanceManager != null)
        {
            characterAppearanceManager.ChangeAppearance(appearances[currentAppearanceIndex]);
        }
        else
        {
            Debug.LogError("No hay apariencias en la lista o CharacterAppearanceManager no está asignado.");
        }
    }
}
