using Cinemachine;
using UnityEngine;

public class CursorZoom : MonoBehaviour
{
    [SerializeField] private float minZoom = 6f;
    [SerializeField] private float maxZoom = 1f;
    [SerializeField] private float smooth = 4f;
    [SerializeField] private float zoomSensitivity = 1f; // Valor (multiplicador) bajo para que por cada movimiento scroll que haga, no sea muy fuerte el cambio.
    private float defaultDistance = 6f;

    private float newDistanceCam;

    private CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        newDistanceCam = defaultDistance;
    }

    void Update()
    {
        CameraZoom();
    }

    public void CameraZoom()
    {
        float zoomValue = -Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity; // El "-Input" es para que invierta el scroll.
        newDistanceCam = Mathf.Clamp(newDistanceCam + zoomValue, minZoom, maxZoom);
        float currentDistance = framingTransposer.m_CameraDistance;
        if (currentDistance == newDistanceCam)
        {
            return;
        }

        float lerpedZoomValue = Mathf.Lerp(currentDistance, newDistanceCam, smooth * Time.deltaTime);
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}
