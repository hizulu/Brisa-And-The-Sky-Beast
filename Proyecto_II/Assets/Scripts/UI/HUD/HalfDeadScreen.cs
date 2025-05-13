using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* NOMBRE CLASE: HalfDeadScreen
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 09/05/2025
 * DESCRIPCI�N: Script que gestiona la pantalla de medio muerto. 
 * VERSI�N: 1.0 Solo l�gica para el panel de timer screen 
 * 1.1 A�adido el panel de revivir.
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

    //M�todo para mostrar la pantalla de medio muerto de Brisa. Se a�ade el cursor porque hay un boton para rendirse.
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

    // M�todo para mostrar la pantalla de mientras la Bestia est� reviviendo a Brisa.
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

    // M�todo para mostrar la pantalla de medio muerto de la Bestia.
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

    // M�todo para mostrar la pantalla de mientras Brisa est� reviviendo a la Bestia.
    public void ShowHalfDeadScreenBestiaRevive(float revivingTime)
    {
        halfDeadScreenBestiaRevive.SetActive(true);
        timerScreenBestia.SetActive(false);
        revivingCircleBestia.fillAmount = revivingTime;

        // Configuraci�n inicial con el color #6F3939
        blurMaterial.SetFloat("_BlurSize", 2f);
        blurMaterial.SetColor("_Color", initialColor);
        blurMaterial.SetFloat("_Brightness", 2f);

        StartTransition();
    }

    // M�todos para ocultar las pantallas de medio muerta de Brisa
    public void HideHalfDeadScreenBrisa()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        halfDeadScreenBrisa.SetActive(false);
    }

    // M�todo para ocultar la pantalla de revivir de la Bestia
    public void HideHalfDeadScreenBestia()
    {
        halfDeadScreenBestia.SetActive(false);
    }

    //Metodo para comenzar la animaci�n de transici�n del desenfoque
    private void StartTransition()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionBlurEffect());
    }

    // M�todo para iniciar la transici�n del desenfoque a el color normal
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