using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlSettings : MonoBehaviour
{
    [Header("Sensibilidad del ratón")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private float minSensitivity = 0.1f;
    [SerializeField] private float maxSensitivity = 5f;
    [SerializeField] private float defaultSensitivity = 1f;

    [SerializeField] private CursorZoom mainCameraZoom; // Script en la cámara principal
    [SerializeField] private MapNavigation mapNavigation; // Script en el mapa

    private void Start()
    {
        SetupSlider();
    }

    private void SetupSlider()
    {
        if (mouseSensitivitySlider == null) return;

        mouseSensitivitySlider.minValue = minSensitivity;
        mouseSensitivitySlider.maxValue = maxSensitivity;
        mouseSensitivitySlider.value = defaultSensitivity;

        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        SetMouseSensitivity(defaultSensitivity); // Aplicar valor inicial
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        if (mainCameraZoom != null)
            mainCameraZoom.zoomSensitivity = sensitivity;

        if (mapNavigation != null)
            mapNavigation.dragSpeed = sensitivity * 5f; // Ajuste proporcional para mantener sensación suave
    }
}
