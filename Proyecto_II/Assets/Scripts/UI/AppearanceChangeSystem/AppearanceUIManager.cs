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
 * VERSIÓN: 1.0 Apariencia inicial, cambio de apariencia a la derecha e izquierda.
 * 1.1 Cambio de apariencia al hacer clic en el botón.
 * 1.2 Cambios en la interfaz de usuario para mostrar información de la apariencia bloqueada y desbloqueada. 
 *     Toma la cantidad del objeto necesario del InventoryManager.
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
            //Debug.Log("UI. Bandera 1");
            return;
        }

        // Cambiar la imagen principal
        if (appearanceImage != null && newAppearance.appearanceIcon != null)
        {
            appearanceImage.sprite = newAppearance.appearanceIcon;
            //Debug.Log("UI. Bandera 2");
        }
        else
        {
            //Debug.LogError("appearanceImage no está asignado o appearanceIcon es NULL.");
        }

        if (newAppearance.isUnlocked)
        {
            //Debug.Log("UI. Bandera 3");
            // Mostrar panel desbloqueado y ocultar bloqueado
            unlockedSkinPanel.SetActive(true);
            //Debug.Log("UI. Bandera 4");
            blockedSkinPanel.SetActive(false);
            //Debug.Log("UI. Bandera 5");
            selectSkinButton.interactable = true;
            //Debug.Log("UI. Bandera 6");

            // Actualizar textos
            if (unlockedNameText != null) unlockedNameText.text = newAppearance.appearanceName;
            //Debug.Log("UI. Bandera 7");
            if (unlockedDescriptionText != null) unlockedDescriptionText.text = newAppearance.appearanceDescription;
            //Debug.Log("UI. Bandera 8");
        }
        else
        {
            //Debug.Log("UI. Bandera 9");
            // Mostrar panel bloqueado y ocultar desbloqueado
            unlockedSkinPanel.SetActive(false);
            //Debug.Log("UI. Bandera 10");
            blockedSkinPanel.SetActive(true);
            //Debug.Log("UI. Bandera 11");
            selectSkinButton.interactable = false;
            //Debug.Log("UI. Bandera 12");

            // Actualizar textos
            if (blockedNameText != null) blockedNameText.text = newAppearance.appearanceName;
            //Debug.Log("UI. Bandera 13");
            if (blockedDescriptionText != null) blockedDescriptionText.text = newAppearance.appearanceDescription;
            //Debug.Log("UI. Bandera 14");
            if (objectsNeededText != null) objectsNeededText.text = $"Objetos necesarios: {newAppearance.objectsNeeded}";
            //Debug.Log("UI. Bandera 15");
            objectsObtainedQuantityText.text = InventoryManager.Instance.GetItemQuantity(newAppearance.objectsNeededPrefab).ToString();
            //Debug.Log("UI. Bandera 16");
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
