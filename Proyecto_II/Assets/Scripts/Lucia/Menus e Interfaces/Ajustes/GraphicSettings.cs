using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/* NOMBRE CLASE: GraphicsSettings
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 23/04/2025
 * DESCRIPCI�N: Script que gestiona los ajustes gr�ficos del juego. Permite ajustar el brillo, la resoluci�n y el modo de pantalla.
 * VERSI�N: 1.0 Sistema de ajustes gr�ficos inicial.
 * 1.1 Se ha a�adido la opci�n de cambiar el brillo al sistema SunController creado por Sara.
 * 1.2 El brillo tambi�n afecta al UI. Se utiliza un shader.
 */

public class GraphicsSettings : MonoBehaviour
{
    [Header("Brillo")]
    [SerializeField] private SunController sunController;
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

    [Header("Resoluci�n")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int defaultResolutionIndex = 2;
    private Dictionary<Graphic, Material> originalGraphicMaterials = new Dictionary<Graphic, Material>();
    private Dictionary<TMP_Text, Color> originalTextColors = new Dictionary<TMP_Text, Color>();
    private Dictionary<Mask, Material> originalMaskMaterials = new Dictionary<Mask, Material>();
    private Material sharedBrightnessMaterial;

    private void Awake()
    {
        if (sunController == null)
        {
            sunController = FindObjectOfType<SunController>();
            if (sunController == null && directionalLight != null)
            {
                sunController = directionalLight.GetComponent<SunController>();
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

            // Almacenar materiales originales de UI est�ndar
            Graphic[] allGraphics = canvas.GetComponentsInChildren<Graphic>(true);
            foreach (Graphic graphic in allGraphics)
            {
                if (graphic == null) continue;

                if (excludedElements.Contains(graphic)) continue;

                // Manejar elementos UI est�ndar (Image, RawImage, etc.)
                if (!(graphic is TMP_Text) && !originalGraphicMaterials.ContainsKey(graphic))
                {
                    originalGraphicMaterials[graphic] = graphic.material;
                }

                // Manejar m�scaras
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
        // Restaurar UI est�ndar
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

        // Restaurar m�scaras
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
        // 1. Actualizar elementos UI est�ndar (Image, RawImage, Panel, etc.)
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

    // M�todos para configuraci�n de pantalla (sin cambios)
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

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(new List<string> {
            "3840 x 2160",
            "2560 x 1440",
            "1920 x 1080",
            "1600 x 900",
            "1366 x 768",
            "1280 x 720"
        });

        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", defaultResolutionIndex);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        SetResolution(resolutionDropdown.value);
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

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("Resolution", index);
        Vector2Int[] resolutions = new Vector2Int[]
        {
            new Vector2Int(3840, 2160),
            new Vector2Int(2560, 1440),
            new Vector2Int(1920, 1080),
            new Vector2Int(1600, 900),
            new Vector2Int(1366, 768),
            new Vector2Int(1280, 720)
        };

        if (index >= 0 && index < resolutions.Length)
        {
            Vector2Int res = resolutions[index];
            Screen.SetResolution(res.x, res.y, Screen.fullScreenMode);
        }
    }
}