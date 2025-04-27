using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class ControlSettings : MonoBehaviour
{
    [Header("Sensibilidad del ratón")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private float minSensitivity = 0.1f;
    [SerializeField] private float maxSensitivity = 5f;
    [SerializeField] private float defaultSensitivity = 1f;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineVirtualCamera playerCam;
    private CinemachinePOV cinemachinePOV;

    [Header("Mapa")]
    [SerializeField] private MapNavigation mapNavigation;

    private float currentSensitivity;
    private float baseDragSpeed; // Para almacenar el valor original

    private void Awake()
    {
        // Validación de referencias
        if (playerCam != null)
        {
            cinemachinePOV = playerCam.GetCinemachineComponent<CinemachinePOV>();
            if (cinemachinePOV == null)
            {
                Debug.LogError("No se encontró componente CinemachinePOV en la cámara virtual", this);
            }
        }

        if (mapNavigation != null)
        {
            baseDragSpeed = mapNavigation.dragSpeed;
        }
    }

    private void Start()
    {
        InitializeSlider();
        currentSensitivity = defaultSensitivity;
        ApplySensitivity(currentSensitivity);
    }

    private void InitializeSlider()
    {
        if (mouseSensitivitySlider == null) return;

        mouseSensitivitySlider.minValue = minSensitivity;
        mouseSensitivitySlider.maxValue = maxSensitivity;
        mouseSensitivitySlider.value = defaultSensitivity;
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);

        UpdateSensitivityText(defaultSensitivity);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        currentSensitivity = sensitivity;
        ApplySensitivity(sensitivity);
        UpdateSensitivityText(sensitivity);
    }

    private void ApplySensitivity(float sensitivity)
    {
        // Aplicar a Cinemachine
        if (cinemachinePOV != null)
        {
            cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = 200f * sensitivity;
            cinemachinePOV.m_VerticalAxis.m_MaxSpeed = 200f * sensitivity;
        }

        // Aplicar al mapa
        if (mapNavigation != null)
        {
            mapNavigation.dragSpeed = baseDragSpeed * sensitivity;
        }
    }

    private void UpdateSensitivityText(float sensitivity)
    {
        if (mouseSensitivityText != null)
        {
            mouseSensitivityText.text = sensitivity.ToString("0.0");
        }
    }
}