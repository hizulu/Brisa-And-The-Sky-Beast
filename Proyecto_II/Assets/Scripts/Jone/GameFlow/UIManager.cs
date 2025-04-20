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
        return true;
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
