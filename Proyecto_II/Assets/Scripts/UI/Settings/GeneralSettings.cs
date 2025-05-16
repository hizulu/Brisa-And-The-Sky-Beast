using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOMBRE CLASE: GeneralSettings
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 16/05/2025
 * DESCRIPCI�N: Script que gestiona los ajustes generales del juego. Permite abrir y cerrar los paneles de configuraci�n de audio, gr�ficos y controles.
 * VERSI�N: 1.0
 */

public class GeneralSettings : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject generalSettingsPanel;
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject graphicSettingsPanel;
    [SerializeField] private GameObject controlsSettingsPanel;
    #endregion

    #region Singleton
    public static GeneralSettings Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void CloseSettings()
    {
        generalSettingsPanel.SetActive(true);
        audioSettingsPanel.SetActive(false);
        graphicSettingsPanel.SetActive(false);
        controlsSettingsPanel.SetActive(false);
    }
}
