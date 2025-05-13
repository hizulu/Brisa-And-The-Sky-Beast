using Cinemachine;
using UnityEngine;

/* NOMBRE CLASE: CursorZoom
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/11/2024
 * DESCRIPCI�N: Script que hace que haciendo scroll con el rat�n, haga zoom in/out.
 * VERSI�N: 1.0 Acci�n de zoom in/out
 */

public class CursorZoom : MonoBehaviour
{
    [SerializeField] private float minZoom = 6f;
    [SerializeField] private float maxZoom = 1f;
    [SerializeField] private float smooth = 4f;
    [SerializeField] public float zoomSensitivity = 1f; // Ajuste de sensibilidad
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

    /* NOMBRE M�TODO: CameraZoom
 * AUTOR: Sara Yue Madruga Mart�n
 * FECHA: 09/11/2024
 * DESCRIPCI�N: m�todo que gestiona el zoom in/out hacia el player con el bot�n central del rat�n (haciendo scroll).
                suma 
 * @param: -
 * @return: - 
 */

    public void CameraZoom()
    {
        // Agrega un ajuste m�s notable de sensibilidad
        float zoomValue = -Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        newDistanceCam = Mathf.Clamp(newDistanceCam + zoomValue, minZoom, maxZoom);

        // Aseg�rate de que la distancia no sea la misma para evitar cambios innecesarios
        float currentDistance = framingTransposer.m_CameraDistance;
        if (currentDistance == newDistanceCam)
        {
            return;
        }

        // Cambia el valor de suavizado para una transici�n m�s fluida
        float lerpedZoomValue = Mathf.Lerp(currentDistance, newDistanceCam, smooth * Time.deltaTime);
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}
