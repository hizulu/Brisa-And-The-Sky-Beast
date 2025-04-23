using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    [Header("Brillo")]
    [SerializeField] private Light directionalLight; // Asignar luz direccional
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private float minBrightness = 0.2f;
    [SerializeField] private float maxBrightness = 2f;

    [Header("Pantalla")]
    [SerializeField] private TMP_Dropdown screenModeDropdown;

    [Header("Resolución")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;
    private int defaultResolutionIndex;

    private void Start()
    {
        SetupBrightnessSlider();
        SetupScreenModes();
        SetupResolutions();
    }

    private void SetupBrightnessSlider()
    {
        if (brightnessSlider != null && directionalLight != null)
        {
            brightnessSlider.minValue = minBrightness;
            brightnessSlider.maxValue = maxBrightness;
            brightnessSlider.value = directionalLight.intensity;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
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

        screenModeDropdown.value = 0; // Por defecto: pantalla completa
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        SetScreenMode(0);
    }

    private void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        defaultResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            Resolution res = availableResolutions[i];
            string option = res.width + " x " + res.height;
            options.Add(option);

            if (res.width == 1920 && res.height == 1080)
                defaultResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = defaultResolutionIndex;
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        SetResolution(defaultResolutionIndex);
    }

    public void SetBrightness(float value)
    {
        if (directionalLight != null)
            directionalLight.intensity = value;
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
        if (index < 0 || index >= availableResolutions.Length) return;

        Resolution res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }
}
