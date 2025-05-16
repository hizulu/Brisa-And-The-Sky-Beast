using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/* NOMBRE CLASE: GraphicsSettings
 * AUTOR: Lucía García López
 * FECHA: 23/04/2025
 * DESCRIPCIÓN: Script que gestiona los ajustes gráficos del juego. Permite ajustar el brillo, la resolución y el modo de pantalla.
 * VERSIÓN: 1.0 Sistema de ajustes gráficos inicial.
 * 1.1 Se ha añadido la opción de cambiar el brillo al sistema SunController creado por Sara.
 * 1.2 El brillo también afecta al UI. Se utiliza un shader.
 * 1.3 Cambio de resolución funciona correctamente.
 */

public class GraphicsSettings : MonoBehaviour
{
    #region Variables
    [Header("Brillo")]
    [SerializeField] private SunManager sunController;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Text brightnessValueText;
    [SerializeField] private float minBrightness = 0.3f;
    [SerializeField] private float maxBrightness = 2f;
    [SerializeField] private float defaultBrightness = 1f;

    [Header("Paneles UI")]
    [SerializeField] private List<Canvas> targetCanvas = new List<Canvas>();
    [SerializeField] private List<Graphic> excludedElements = new List<Graphic>();
    [SerializeField] private Material uiBrightnessMaterial;

    [Header("Pantalla")]
    [SerializeField] private TMP_Dropdown screenModeDropdown;

    [Header("Resolución")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;

    private Dictionary<Graphic, Material> originalGraphicMaterials = new Dictionary<Graphic, Material>();
    private Dictionary<TMP_Text, Color> originalTextColors = new Dictionary<TMP_Text, Color>();
    private Dictionary<Mask, Material> originalMaskMaterials = new Dictionary<Mask, Material>();
    private Material sharedBrightnessMaterial;
    #endregion

    private void Awake()
    {
        if (sunController == null)
        {
            sunController = FindObjectOfType<SunManager>();
            if (sunController == null && directionalLight != null)
            {
                sunController = directionalLight.GetComponent<SunManager>();
            }
        }

        if (uiBrightnessMaterial != null)
        {
            sharedBrightnessMaterial = new Material(uiBrightnessMaterial);
        }
    }

    private void Start()
    {
        SetupBrightnessSlider();
        SetupScreenModes();
        SetupResolutions();
        StoreOriginalColorsAndMaterials();
        UpdateUIBrightness(brightnessSlider.value);
    }

    private void OnDestroy()
    {
        RestoreOriginalColorsAndMaterials();
    }

    private void StoreOriginalColorsAndMaterials()
    {
        originalGraphicMaterials.Clear();
        originalTextColors.Clear();
        originalMaskMaterials.Clear();

        foreach (Canvas canvas in targetCanvas)
        {
            if (canvas == null) continue;

            // Almacenar materiales originales de UI estándar
            Graphic[] allGraphics = canvas.GetComponentsInChildren<Graphic>(true);
            foreach (Graphic graphic in allGraphics)
            {
                if (graphic == null) continue;

                if (excludedElements.Contains(graphic)) continue;

                // Manejar elementos UI estándar (Image, RawImage, etc.)
                if (!(graphic is TMP_Text) && !originalGraphicMaterials.ContainsKey(graphic))
                {
                    originalGraphicMaterials[graphic] = graphic.material;
                }

                // Manejar máscaras
                Mask mask = graphic.GetComponent<Mask>();
                if (mask != null && !originalMaskMaterials.ContainsKey(mask))
                {
                    originalMaskMaterials[mask] = mask.graphic.material;
                }
            }

            // Almacenar colores originales de TextMeshPro
            TMP_Text[] textMeshPros = canvas.GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text text in textMeshPros)
            {
                if (text != null && !originalTextColors.ContainsKey(text))
                {
                    // Guardar el color original (ya sea personalizado o blanco)
                    originalTextColors[text] = text.color;
                }
            }
        }
    }

    private void RestoreOriginalColorsAndMaterials()
    {
        // Restaurar UI estándar
        foreach (var kvp in originalGraphicMaterials)
        {
            if (kvp.Key != null) kvp.Key.material = kvp.Value;
        }

        // Restaurar TextMeshPro
        foreach (var kvp in originalTextColors)
        {
            if (kvp.Key != null)
            {
                kvp.Key.color = kvp.Value;
                kvp.Key.ForceMeshUpdate();
            }
        }

        // Restaurar máscaras
        foreach (var kvp in originalMaskMaterials)
        {
            if (kvp.Key != null && kvp.Key.graphic != null)
            {
                kvp.Key.graphic.material = kvp.Value;
            }
        }
    }

    private void SetupBrightnessSlider()
    {
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = minBrightness;
            brightnessSlider.maxValue = maxBrightness;
            brightnessSlider.value = defaultBrightness;
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            UpdateBrightnessText(brightnessSlider.value);
        }
    }

    private void UpdateBrightnessText(float value)
    {
        if (brightnessValueText != null)
            brightnessValueText.text = value.ToString("0.##");
    }

    public void SetBrightness(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);

        // Actualizar brillo de la luz del sol
        if (sunController != null)
        {
            sunController.SetLightIntensityMultiplier(value);
        }

        UpdateUIBrightness(value);
        UpdateBrightnessText(value);
    }

    private void UpdateUIBrightness(float brightnessValue)
    {
        // 1. Actualizar elementos UI estándar (Image, RawImage, Panel, etc.)
        if (sharedBrightnessMaterial != null)
        {
            sharedBrightnessMaterial.SetFloat("_Brightness", brightnessValue);

            foreach (var kvp in originalGraphicMaterials)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.material = sharedBrightnessMaterial;
                }
            }
        }

        // 2. Actualizar TextMeshPro - Aplicar brillo como multiplicador al color ORIGINAL
        foreach (var kvp in originalTextColors)
        {
            if (kvp.Key != null)
            {
                // Obtener el color original (ya sea personalizado o blanco)
                Color originalColor = kvp.Value;

                // Aplicar brillo como multiplicador, manteniendo el alpha original
                Color adjustedColor = new Color(
                    originalColor.r * brightnessValue,
                    originalColor.g * brightnessValue,
                    originalColor.b * brightnessValue,
                    originalColor.a  // Mantener el alpha original
                );

                kvp.Key.color = adjustedColor;
                kvp.Key.ForceMeshUpdate();
            }
        }

        // 3. Actualizar elementos con Mask
        foreach (var kvp in originalMaskMaterials)
        {
            if (kvp.Key != null && kvp.Key.graphic != null && sharedBrightnessMaterial != null)
            {
                kvp.Key.graphic.material = sharedBrightnessMaterial;
            }
        }
    }

    private void SetupScreenModes()
    {
        if (screenModeDropdown == null) return;

        screenModeDropdown.ClearOptions();
        screenModeDropdown.AddOptions(new List<string> {
            "Pantalla completa",
            "Ventana",
            "Sin bordes"
        });

        screenModeDropdown.value = PlayerPrefs.GetInt("ScreenMode", 0);
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        SetScreenMode(screenModeDropdown.value);
    }

    private void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        // Filtrar resoluciones únicas (sin duplicados)
        foreach (Resolution res in resolutions)
        {
            if (!filteredResolutions.Exists(r => r.width == res.width && r.height == res.height))
            {
                filteredResolutions.Add(res);
            }
        }

        // Ordenar de mayor a menor resolución
        filteredResolutions.Sort((a, b) => b.width.CompareTo(a.width));

        // Preparar opciones para el Dropdown
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            options.Add(resolutionOption);

            // Detectar la resolución actual
            if (filteredResolutions[i].width == Screen.width &&
                filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
    }

    public void SetScreenMode(int index)
    {
        PlayerPrefs.SetInt("ScreenMode", index);
        FullScreenMode mode = FullScreenMode.FullScreenWindow;
        switch (index)
        {
            case 0: mode = FullScreenMode.FullScreenWindow; break;
            case 1: mode = FullScreenMode.Windowed; break;
            case 2: mode = FullScreenMode.MaximizedWindow; break;
        }
        Screen.fullScreenMode = mode;
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count) return;

        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        currentResolutionIndex = resolutionIndex;
    }
}