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

    [Header("Material References")]
    [SerializeField] private Material blurMaterial;
    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private float targetBlurAmount = 0f;
    [SerializeField] private float targetBrightness = 1f;

    // Colores definidos como constantes
    private readonly Color initialColor = new Color(0.435f, 0.224f, 0.224f, 1f); // #6F3939
    private readonly Color targetColor = Color.white;

    private Coroutine transitionCoroutine;
    #endregion

    //Método para mostrar la pantalla de medio muerto de Brisa. Se añade el cursor porque hay un boton para rendirse.
    public void ShowHalfDeadScreenBrisa(float currentTime, float maxTime)
    {
        ResetBlurEffect();
        halfDeadScreenBrisa.SetActive(true);
        timerScreenBrisa.SetActive(true);

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

        //Se configura el material de desenfoque dedl fondo
        blurMaterial.SetFloat("_BlurSize", 2f);
        blurMaterial.SetColor("_Color", initialColor);
        blurMaterial.SetFloat("_Brightness", 2f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartTransition();
    }

    // Método para mostrar la pantalla de medio muerto de la Bestia.
    public void ShowHalfDeadScreenBestia (float currentTime, float maxTime)
    {
        ResetBlurEffect();
        halfDeadScreenBestia.SetActive(true);
        timerScreenBestia.SetActive(true);
        // Configurar slider correctamente
        timerSliderBestia.minValue = 0;
        timerSliderBestia.maxValue = maxTime;
        timerSliderBestia.value = currentTime;
        halfDeadScreenTextBestia.text = currentTime.ToString("00");
    }

    // Método para mostrar la pantalla de mientras Brisa está reviviendo a la Bestia.
    public void ShowHalfDeadScreenBestiaRevive(float revivingTime)
    {
        halfDeadScreenBestiaRevive.SetActive(true);
        timerScreenBestia.SetActive(false);
        revivingCircleBestia.fillAmount = revivingTime;

        // Configuración inicial con el color #6F3939
        blurMaterial.SetFloat("_BlurSize", 2f);
        blurMaterial.SetColor("_Color", initialColor);
        blurMaterial.SetFloat("_Brightness", 2f);

        StartTransition();
    }

    // Métodos para ocultar las pantallas de medio muerta de Brisa
    public void HideHalfDeadScreenBrisa()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        halfDeadScreenBrisa.SetActive(false);
    }

    // Método para ocultar la pantalla de revivir de la Bestia
    public void HideHalfDeadScreenBestia()
    {
        halfDeadScreenBestia.SetActive(false);
    }

    //Metodo para comenzar la animación de transición del desenfoque
    private void StartTransition()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionBlurEffect());
    }

    // Método para iniciar la transición del desenfoque a el color normal
    private IEnumerator TransitionBlurEffect()
    {
        float elapsedTime = 0f;
        float initialBlur = blurMaterial.GetFloat("_BlurSize");
        Color currentColor = blurMaterial.GetColor("_Color");
        float initialBrightness = blurMaterial.GetFloat("_Brightness");

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            blurMaterial.SetFloat("_BlurSize", Mathf.Lerp(initialBlur, targetBlurAmount, smoothT));
            blurMaterial.SetColor("_Color", Color.Lerp(currentColor, targetColor, smoothT));
            blurMaterial.SetFloat("_Brightness", Mathf.Lerp(initialBrightness, targetBrightness, smoothT));

            yield return null;
        }

        blurMaterial.SetFloat("_BlurSize", targetBlurAmount);
        blurMaterial.SetColor("_Color", targetColor);
        blurMaterial.SetFloat("_Brightness", targetBrightness);

        transitionCoroutine = null;
    }

    //Metodo para reiniciar el efecto de desenfoque
    public void ResetBlurEffect()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        // Restablecer a los valores iniciales (#6F3939)
        blurMaterial.SetFloat("_BlurSize", 2f);
        blurMaterial.SetColor("_Color", initialColor);
        blurMaterial.SetFloat("_Brightness", 2f);
    }
}