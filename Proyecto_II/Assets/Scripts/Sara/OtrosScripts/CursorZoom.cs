using Cinemachine;
using UnityEngine;

/* NOMBRE CLASE: Cursor Zoom
 * AUTOR: Sara Yue Madruga Martín
 * FECHA: 09/11/2024
 * DESCRIPCIÓN: Script que hace que haciendo scroll con el ratón, haga zoom in/out.
 * VERSIÓN: 1.0 Acción de zoom in/out
 */

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CameraZoom();
    }

    /* NOMBRE MÉTODO: CameraZoom
     * AUTOR: Sara Yue Madruga Martín
     * FECHA: 09/11/2024
     * DESCRIPCIÓN: método que gestiona el zoom in/out hacia el player con el botón central del ratón (haciendo scroll).
                    suma 
     * @param: -
     * @return: - 
     */
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
