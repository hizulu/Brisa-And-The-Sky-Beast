using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* NOMBRE CLASE: UIManager
 * AUTOR: Jone Sainz Egea
 * FECHA: 12/03/2025
 * DESCRIPCIÓN: Script base que se encarga del flujo del juego
 * VERSIÓN: 1.0 estructura básica de singleton
 */
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject victoryPanel;

    [field: Header("Panels for closing")]
    [SerializeField] GameObject beastSelectionPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject signPanel;

    [SerializeField] DialogManager diaManager;
    [SerializeField] SignTextManager signTextManager;

    // Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public bool CheckForOpenedMenus()
    {
        if (InventoryManager.Instance.inventoryEnabled)
        {
            Debug.Log("Inventario abierto, no debería pausar");
            InventoryManager.Instance.CloseInventory();
            return true;
        }

        if (CinematicsManager.CineReproduciendo)
        {
            CinematicsManager.Instance.TogglePauseResume();
            return true;
        }

        if (mapPanel.activeInHierarchy)
        {
            MapManager mapManager = mapPanel.GetComponent<MapManager>();
            mapManager.ClosePanel();
            return true;
        }

        if (settingsPanel.activeInHierarchy)
        {
            GeneralSettings.Instance.CloseSettings();
            settingsPanel.SetActive(false);
            return false; // Se gestiona desde pause
        }

        if (dialoguePanel.activeInHierarchy)
        {
            diaManager.CloseDialog();
            return true;
        }

        if (signPanel.activeInHierarchy)
        {
            signTextManager.CloseSignPanel();
            return true;
        }

        return false; // No había ningún panel abierto
    }

    #region Pause & Resume
    public void OpenPauseMenu()
    {
        ActivateCursor();
        pausePanel.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        DeactivateCursor();
        pausePanel.SetActive(false);
    }
    #endregion

    #region GameOver
    public void OpenGameOverMenu()
    {
        ActivateCursor();
        gameOverPanel.SetActive(true);
    }

    public void CloseGameOverMenu()
    {
        DeactivateCursor();
        gameOverPanel.SetActive(false);
    }
    #endregion

    #region Victory
    public void OpenVictoryMenu()
    {
        ActivateCursor();
        victoryPanel.SetActive(true);
    }

    public void CloseVictoryMenu()
    {
        DeactivateCursor();
        victoryPanel.SetActive(false);
    }
    #endregion

    public void ActivateCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DeactivateCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
