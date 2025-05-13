using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

/* NOMBRE CLASE: ControlSettings
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 23/04/2025
 * DESCRIPCI�N: Script que gestiona los ajustes de control del juego. Permite modificar la sensibilidad del rat�n y la velocidad de arrastre del mapa.
 * VERSI�N: 1.0
 */

public class ControlSettings : MonoBehaviour
{
    #region Variables
    [Header("Sensibilidad del rat�n")]
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
    #endregion

    private void Awake()
    {
        if (playerCam != null)
        {
            cinemachinePOV = playerCam.GetCinemachineComponent<CinemachinePOV>();
            if (cinemachinePOV == null)
            {
                Debug.LogError("No se encontr� componente CinemachinePOV en la c�mara virtual", this);
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