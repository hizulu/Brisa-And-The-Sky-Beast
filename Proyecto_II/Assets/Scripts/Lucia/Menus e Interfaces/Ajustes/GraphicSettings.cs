using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    [Header("Brillo")]
    public float lightIntensityMultiplier;
    [SerializeField] private SunController sunController;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Text brightnessValueText;
    [SerializeField] private float minBrightness = 0.2f;
    [SerializeField] private float maxBrightness = 2f;

    [Header("Pantalla")]
    [SerializeField] private TMP_Dropdown screenModeDropdown;

    [Header("Resolución")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int defaultResolutionIndex = 2;

    private void Awake()
    {
        // Asegurarse de que tenemos referencia al SunController
        if (sunController == null)
        {
            // Intentar encontrar automáticamente el SunController en la escena
            sunController = FindObjectOfType<SunController>();
            
            // Si aún es null, buscar en la luz direccional
            if (sunController == null && directionalLight != null)
            {
                sunController = directionalLight.GetComponent<SunController>();
            }
        }
    }

    private void Start()
    {
        SetupBrightnessSlider();
        SetupScreenModes();
        SetupResolutions();
    }

    private void SetupBrightnessSlider()
    {
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = minBrightness;
            brightnessSlider.maxValue = maxBrightness;
            brightnessSlider.value = 1f;
            UpdateBrightnessText(brightnessSlider.value);
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    private void UpdateBrightnessText(float value)
    {
        if (brightnessValueText != null)
            brightnessValueText.text = value.ToString("0.##");
    }

    private void SetupScreenModes()
    {
        if (screenModeDropdown == null) return;

        screenModeDropdown.ClearOptions();
        screenModeDropdown.AddOptions(new System.Collections.Generic.List<string> {
            "Pantalla completa",
            "Ventana",
            "Sin bordes"
        });

        screenModeDropdown.value = 0;
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        SetScreenMode(0);
    }

    private void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutionDropdown.ClearOptions();

        resolutionDropdown.AddOptions(new System.Collections.Generic.List<string> {
            "3840 x 2160",
            "2560 x 1440",
            "1920 x 1080",
            "1600 x 900",
            "1366 x 768",
            "1280 x 720"
        });

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        resolutionDropdown.value = defaultResolutionIndex;
        SetResolution(defaultResolutionIndex);
    }

    public void SetBrightness(float value)
    {
        if (sunController != null)
        {
            sunController.SetLightIntensityMultiplier(value);
        }

        UpdateBrightnessText(value);
    }

    public void SetScreenMode(int index)
    {
        FullScreenMode mode = FullScreenMode.FullScreenWindow;

        switch (index)
        {
            case 0: mode = FullScreenMode.FullScreenWindow; break;
            case 1: mode = FullScreenMode.Windowed; break;
            case 2: mode = FullScreenMode.MaximizedWindow; break;
        }

        Screen.fullScreenMode = mode;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(3840, 2160, Screen.fullScreenMode); break;
            case 1: Screen.SetResolution(2560, 1440, Screen.fullScreenMode); break;
            case 2: Screen.SetResolution(1920, 1080, Screen.fullScreenMode); break;
            case 3: Screen.SetResolution(1600, 900, Screen.fullScreenMode); break;
            case 4: Screen.SetResolution(1366, 768, Screen.fullScreenMode); break;
            case 5: Screen.SetResolution(1280, 720, Screen.fullScreenMode); break;
        }
    }
}