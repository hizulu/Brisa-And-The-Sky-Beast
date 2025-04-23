using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class ControlSettings : MonoBehaviour
{
    [Header("Sensibilidad del ratón")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private float minSensitivity = 0.1f;
    [SerializeField] private float maxSensitivity = 5f;
    [SerializeField] private float defaultSensitivity = 1f;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    private float currentSensitivity;

    // Referencia a la Cinemachine FreeLook Camera
    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineFreeLook cinemachineFreeLookCamera;

    private void Start()
    {
        SetupSlider();
        currentSensitivity = defaultSensitivity;
    }

    private void SetupSlider()
    {
        if (mouseSensitivitySlider == null) return;

        mouseSensitivitySlider.minValue = minSensitivity;
        mouseSensitivitySlider.maxValue = maxSensitivity;
        mouseSensitivitySlider.value = defaultSensitivity;

        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        currentSensitivity = sensitivity;

        // Mostrar el valor redondeado en el texto
        if (mouseSensitivityText != null)
            mouseSensitivityText.text = sensitivity.ToString("0.0");

        // Ajustar m_MaxSpeed de Cinemachine para aplicar la sensibilidad
        if (cinemachineFreeLookCamera != null)
        {
            // Asignar la sensibilidad a los valores de velocidad máxima
            cinemachineFreeLookCamera.m_XAxis.m_MaxSpeed = 200f * sensitivity; // Velocidad en el eje horizontal
            cinemachineFreeLookCamera.m_YAxis.m_MaxSpeed = 200f * sensitivity; // Velocidad en el eje vertical
        }
    }
}
