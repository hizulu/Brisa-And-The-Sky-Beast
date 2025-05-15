using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* NOMBRE CLASE: HalfDeadScreen
 * AUTOR: Lucía García López
 * FECHA: 09/05/2025
 * DESCRIPCIÓN: Script que gestiona la pantalla de medio muerto. 
 * VERSIÓN: 1.0 Solo lógica para el panel de timer screen 
 * 1.1 Añadido el panel de revivir.
 */

public class HalfDeadScreen : MonoBehaviour
{
    #region Singleton
    public static HalfDeadScreen Instance { get; private set; }
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

    #region Variables
    [Header("UI References")]
    [Header("Brisa References")]
    [SerializeField] private GameObject halfDeadScreenBrisa;
    [SerializeField] private GameObject timerScreenBrisa;
    [SerializeField] private Slider timerSliderBrisa;
    [SerializeField] private TextMeshProUGUI halfDeadScreenTextBrisa;
    [SerializeField] private GameObject halfDeadScreenBrisaRevive;
    [SerializeField] private Image revivingCircleBrisa;
    [Header("Bestia References")]
    [SerializeField] private GameObject halfDeadScreenBestia;
    [SerializeField] private GameObject timerScreenBestia;
    [SerializeField] private Slider timerSliderBestia;
    [SerializeField] private TextMeshProUGUI halfDeadScreenTextBestia;
    [SerializeField] private GameObject halfDeadScreenBestiaRevive;
    [SerializeField] private Image revivingCircleBestia;

    private bool _isReviving = false;
    #endregion

    //Método para mostrar la pantalla de medio muerto de Brisa. Se añade el cursor porque hay un boton para rendirse.
    public void ShowHalfDeadScreenBrisa(float currentTime, float maxTime)
    {
        halfDeadScreenBrisa.SetActive(true);
        timerScreenBrisa.SetActive(true);
        halfDeadScreenBrisaRevive.SetActive(false);

        // Configurar slider correctamente
        timerSliderBrisa.minValue = 0;
        timerSliderBrisa.maxValue = maxTime;
        timerSliderBrisa.value = currentTime;

        halfDeadScreenTextBrisa.text = currentTime.ToString("00");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Método para mostrar la pantalla de mientras la Bestia está reviviendo a Brisa.
    public void ShowHalfDeadScreenBrisaRevive(float revivingTime)
    {
        halfDeadScreenBrisaRevive.SetActive(true);
        timerScreenBrisa.SetActive(false);
        revivingCircleBrisa.fillAmount = revivingTime;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Método para mostrar la pantalla de medio muerto de la Bestia.
    public void ShowHalfDeadScreenBestia(float currentTime, float maxTime)
    {
        if (_isReviving) return;

        halfDeadScreenBestia.SetActive(true);
        timerScreenBestia.SetActive(true);
        halfDeadScreenBestiaRevive.SetActive(false);

        timerSliderBestia.minValue = 0;
        timerSliderBestia.maxValue = maxTime;
        timerSliderBestia.value = currentTime;
        halfDeadScreenTextBestia.text = currentTime.ToString("00");
    }

    // Método para mostrar la pantalla de mientras Brisa está reviviendo a la Bestia.
    public void ShowHalfDeadScreenBestiaRevive(float revivingProgress)
    {        
        halfDeadScreenBestiaRevive.SetActive(true);
        Debug.Log("Timer bestia desactivado: " + timerScreenBestia.activeSelf);
        timerScreenBestia.SetActive(false);
        revivingCircleBestia.fillAmount = revivingProgress;
    }

    // Métodos para ocultar las pantallas de medio muerta de Brisa
    public void HideHalfDeadScreenBrisa()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        halfDeadScreenBrisa.SetActive(false);
        timerScreenBrisa.SetActive(false);
        halfDeadScreenBestiaRevive.SetActive(false);
    }

    // Método para ocultar la pantalla de revivir de la Bestia
    public void HideHalfDeadScreenBestia()
    {
        halfDeadScreenBestia.SetActive(false);
        timerScreenBestia.SetActive(false);
        halfDeadScreenBestiaRevive.SetActive(false);
    }

    public bool IsReviving
    {
        get => _isReviving;
        set => _isReviving = value;
    }
}