using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ControlSettings : MonoBehaviour
{
    [Header("Sensibilidad del rat�n")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private float minSensitivity = 0.1f;
    [SerializeField] private float maxSensitivity = 5f;
    [SerializeField] private float defaultSensitivity = 1f;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;

    private float currentSensitivity;

    // Para controlar la c�mara (esto es solo un ejemplo de c�mo podr�a integrarse)
    [Header("C�mara")]
    [SerializeField] private Transform playerCamera; // Transform de la c�mara
    [SerializeField] private float rotationSpeed = 1f; // Velocidad de rotaci�n

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
    }

    private void Update()
    {
        // Usando el Input System, cambiando la sensibilidad global
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float adjustedX = mouseDelta.x * currentSensitivity;
        float adjustedY = mouseDelta.y * currentSensitivity;

        // Aplicar la sensibilidad al movimiento de la c�mara
        RotateCamera(adjustedX, adjustedY);
    }

    // M�todo para rotar la c�mara
    private void RotateCamera(float mouseX, float mouseY)
    {
        // Rotaci�n alrededor del eje Y (giro de la c�mara hacia la izquierda/derecha)
        float horizontalRotation = mouseX * rotationSpeed;
        playerCamera.Rotate(Vector3.up * horizontalRotation);

        // Rotaci�n alrededor del eje X (giro de la c�mara hacia arriba/abajo)
        float verticalRotation = mouseY * rotationSpeed;
        float currentXRotation = playerCamera.eulerAngles.x;
        float newXRotation = Mathf.Clamp(currentXRotation - verticalRotation, -80f, 80f); // Limitar la rotaci�n vertical
        playerCamera.eulerAngles = new Vector3(newXRotation, playerCamera.eulerAngles.y, 0f);
    }
}
